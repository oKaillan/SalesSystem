namespace SalesSystem.Web.Services;


public class CookieHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var cookie = _httpContextAccessor.HttpContext?
            .Request.Headers["Cookie"]
            .ToString();

        if (!string.IsNullOrEmpty(cookie))
        {
            request.Headers.Add("Cookie", cookie);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
