using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Web.Application.Authentication;
using Web.Application.Repositories.Chat;
using Web.Application.Repositories.Identity;
using Web.Application.Stores;
using Web.Presentation;
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

builder.Services.AddHttpClient("group", (service, client) =>
{
    client.BaseAddress = new Uri("http://localhost:5008");
            
    var store = service.GetRequiredService<TokenStore>();
    
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", store.Jwt);
});

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<TokenStore>();

builder.Services.AddHttpClient(
    CryptoClient.ClientName,
    (service, client) =>
    {
        var store = service.GetRequiredService<TokenStore>();
        
        client.BaseAddress =
            client.BaseAddress = new Uri("http://localhost:5106/graphql");
        
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", store.Jwt);
    });

builder.Services.AddWebSocketClient(
        CryptoClient.ClientName,
    (service, client) =>
    {
        client.Uri =
            client.Uri = new Uri("ws://localhost:5106/graphql");
    });

builder.Services.AddCryptoClient();

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