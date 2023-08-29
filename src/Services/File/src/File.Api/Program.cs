using System.Net;
using System.Text;
using File.Domain;
using File.Domain.Enums;
using File.Domain.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
});

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);
builder.Services.AddSingleton(settings);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.BearerKey)),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/file/{fileName}", ([FromRoute] string fileName) =>
{
    var filePath = Path.Combine(Environment.CurrentDirectory + "/Data", fileName);

    var fileStream = new FileStream(filePath, FileMode.Open);
    
    var contentType = "application/octet-stream";

    var image = new ExtensionRecognition<EImageExtensions>();
    var video = new ExtensionRecognition<EVideoExtensions>();
    var audio = new ExtensionRecognition<EAudioExtensions>();

    if (image.Recognise(fileName) is not null)
    {
        contentType = $"image/{fileName.Split(".")[^1]}";
    }
    else if (video.Recognise(fileName) is not null)
    {
        contentType = $"video/{fileName.Split(".")[^1]}";
    }
    else if (audio.Recognise(fileName) is not null)
    {
        contentType = $"audio/{fileName.Split(".")[^1]}";
    }
    
    return Results.File(fileStream, contentType);
});
app.MapPut("/api/file", async (IFormFile file) =>
{
    if (file.Length <= 0) return Results.BadRequest("No files uploaded.");

    var unescapedFileName = file.FileName;
    var escapedFileName = WebUtility.HtmlEncode(unescapedFileName);

    var splittedName = escapedFileName.Split(".");

    var extension = splittedName[^1];
    var fileName = $"{splittedName[0]}-{Random.Shared.Next(1, 1000000000):000000000}.{extension}";

    var filePath = Path.Combine(Environment.CurrentDirectory + "/Data", fileName);
    await using var stream = new FileStream(filePath, FileMode.Create);
    await file.CopyToAsync(stream);

    return Results.Created(new Uri($"/api/file/{fileName}", UriKind.Relative),  new
    {
        Path = $"api/file/{fileName}",
        FileName = fileName
    });
});

app.MapPost("/api/files", async (List<IFormFile> files) =>
{
    if (files?.Any() == true)
    {
        foreach (var file in files)
        {
            if (file.Length <= 0) continue;
            
            var filePath = Path.Combine(Environment.CurrentDirectory, file.FileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }
        return Results.Ok("Files uploaded successfully.");
    }
    else
    {
        return Results.BadRequest("No files uploaded.");
    }
});

app.UseCors(b => b
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();