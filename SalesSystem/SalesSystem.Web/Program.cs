using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Net.Http.Headers;
using SalesSystem.Web.Components;
using SalesSystem.Web.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient(typeof(BaseApiService<>));
builder.Services.AddTransient<CookieHandler>();
builder.Services.AddTransient<ApiAuthenticationStateProvider>();

builder.Services.AddHttpClient("SSAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["SalesSystemApi"]);
})
.AddHttpMessageHandler<CookieHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider,
    ApiAuthenticationStateProvider>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
