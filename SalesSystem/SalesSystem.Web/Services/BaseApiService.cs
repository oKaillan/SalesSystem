using SalesSystem.Shared.Database.Database.Dtos.FilterDto;
using SalesSystem.Shared.Database.Responses;

namespace SalesSystem.Web.Services
{
    public class BaseApiService<T> where T : class
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BaseApiService<T>> _logger;

        public BaseApiService(IHttpClientFactory factory, ILogger<BaseApiService<T>> logger)
        {
            try
            {
                _httpClient = factory.CreateClient("SSAPI");
                _logger = logger;
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
                //Creating URL
                var url = $"{path}?skip={skip}&take={take}";

                if (filter is null)
                    url += "&descending=false";
                else
                    url += $"&descending={filter!.Desc.ToString()}";

                if (filter?.OrderBy != null)
                    url += $"&orderBy={filter.OrderBy.ToString()}";

                if (!string.IsNullOrEmpty(filter?.Name))
                    url += $"&name={filter.Name}";

                if (filter?.CategoryId != null)
                    url += $"&categoryId={filter.CategoryId}";
                //

                _logger.LogInformation($"Loading {path}");

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Request failed: {response.StatusCode} at {path}");
                }
                else
                {
                    var result = await _httpClient.GetFromJsonAsync<PagedResult<T>>(url);
                    _logger.LogInformation($"{result!.TotalCount} {path} loaded");
                    return result;
                }

                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred trying to request API: {ex.Message}");
                throw;
            }
        }

        public async Task<T> GetObjectById(string path, int id)
        {
            var url = $"{path}/{id}";

            _logger.LogInformation($"Loading {path}");
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Request failed: {response.StatusCode} at {path}");
            }
            else
            {
                var result = await _httpClient.GetFromJsonAsync<T>($"{path}/{id}");

                _logger.LogInformation($"{result} {path} loaded");
                return result!;
            }

            return null!;
        }
    }
}
