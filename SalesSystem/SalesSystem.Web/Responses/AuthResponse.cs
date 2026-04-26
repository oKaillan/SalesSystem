namespace SalesSystem.Web.Responses
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; } = null!;
    }
}
