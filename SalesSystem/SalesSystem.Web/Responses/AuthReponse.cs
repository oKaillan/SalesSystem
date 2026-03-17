namespace SalesSystem.Web.Responses
{
    public class AuthReponse
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; } = null!;
    }
}
