using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using SalesSystem.Web.Components;
using SalesSystem.Web.Services;

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme, options =>
{
        options.LoginPath = "/not-authorized";
        options.AccessDeniedPath = "/not-authorized";
});

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UseAuthorization();
app.UseAuthentication();

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
