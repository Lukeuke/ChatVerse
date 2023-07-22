using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Web.Application.Authentication;
using Web.Application.Repositories.Chat;
using Web.Application.Repositories.Identity;
using Web.Presentation.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddHttpClient("identity", client =>
{
    client.BaseAddress = new Uri("http://localhost:5222");
});

builder.Services.AddHttpClient("group", client =>
{
    client.BaseAddress = new Uri("http://localhost:5008");
});

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();