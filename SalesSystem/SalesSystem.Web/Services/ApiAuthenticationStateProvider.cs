using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using SalesSystem.Shared.Database.Database.AuthDto;
using SalesSystem.Web.Responses;
using System.Security.Claims;

namespace SalesSystem.Web.Services;

public class ApiAuthenticationStateProvider(IHttpClientFactory factory, 
    ILogger<ApiAuthenticationStateProvider> logger) : AuthenticationStateProvider
{

    private readonly HttpClient _httpClient = factory.CreateClient("SSAPI");
    private readonly ILogger<ApiAuthenticationStateProvider> _logger = logger;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        try
        {
            var authPath = "auth/me";
            _logger.LogInformation($"Trying to connect to auth API at route {authPath}");
            var response = await _httpClient.GetFromJsonAsync<AuthDto>(authPath);

            if (response is null)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, response.Name ?? string.Empty),
                new Claim(ClaimTypes.Email, response.Name ?? string.Empty)
            };

            foreach (var role in response.Roles ?? new List<string>())
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", new
        {
            email,
            password
        });

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Logged as {email}");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return new AuthResponse { Success = true };
        }

        return new AuthResponse { Success = false, Errors = ["Wrong Email or Password"] };
    }

    public async Task<AuthResponse> LogoutAsync()
    {
        var response = await _httpClient.PostAsJsonAsync("auth/logout", new
        {

        });

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("User logged out");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return new AuthResponse { Success = true };
        }

        return new AuthResponse { Success = false, Errors = ["Was not possible to Logout user"] };
    }
}
