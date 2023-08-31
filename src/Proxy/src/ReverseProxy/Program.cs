var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy")); ;
builder.Services.AddCors();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => 
{
    endpoints.MapReverseProxy(); 
});

app.UseCors(b => b
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();