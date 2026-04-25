using SalesSystem.Shared.Database.Database.Dtos.FilterDto;
using SalesSystem.Shared.Database.Responses;

namespace SalesSystem.Web.Services
{
    public class BaseApiService<T> where T : class
    {
        private readonly HttpClient _httpClient;

        public BaseApiService(IHttpClientFactory factory)
        {
            try
            {
            _httpClient = factory.CreateClient("SSAPI");
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong trying to initialize HttpClient: {ex.Message}");
            }
        }

        public async Task<PagedResult<T>> GetObjectPagedListAsync(
            string path, 
            int skip, 
            int take, 
            ProductFilterDto? filter = null)
        {
            try
            {
                var url = $"{path}?skip={skip}&take={take}";
                if (!string.IsNullOrEmpty(filter?.Name))
                    url += $"&name={filter.Name}";

                if (filter?.CategoryId != null)
                    url += $"&categoryId={filter.CategoryId}";

                var result = await _httpClient.GetFromJsonAsync<PagedResult<T>>(url);
                if (result is null)
                    throw new Exception($"Object not found at route {path}");

                return result;

            }
            catch (Exception ex) { throw new Exception($"An error has occurred trying to request: {ex.Message}"); }
        }
    }
}
