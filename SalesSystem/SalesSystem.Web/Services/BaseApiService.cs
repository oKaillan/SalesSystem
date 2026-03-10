namespace SalesSystem.Web.Services
{
    public class BaseApiService<T> where T : class
    {
        private readonly HttpClient _httpClient;

        public BaseApiService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("SSAPI");
        }

        public async Task<ICollection<T>> GetObjectListAsync(string path)
        {
            var result = await _httpClient.GetFromJsonAsync<ICollection<T>>(path);
            if (result is null)
                throw new Exception($"Object not found at route {path}");

            return result;

        }
    }
}
