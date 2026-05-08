using SalesSystem.Shared.Database.Interfaces;
using SalesSystem.Shared.Database.Responses;
using System.Reflection;

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

        public async Task<PagedResult<T>> GetObjectPagedListAsync<TOrderBy>(
            string path,
            int skip,
            int take,
            IFilterDto<TOrderBy>? filter = null)
            where TOrderBy : struct
        {
            try
            {
                //Creating URL
                var url = $"{path}?skip={skip}&take={take}";

                if (filter is not null)
                {
                    //creates custom URL depending of Entity
                    var properties = filter.GetType().GetProperties();

                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(filter);

                        if (value == null)
                            continue;

                        var stringValue = value.ToString();

                        if (string.IsNullOrWhiteSpace(stringValue))
                            continue;

                        //Transform URL to LowerCase to match API request
                        var propertyName =
                            char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1);

                        url += $"&{propertyName}={Uri.EscapeDataString(stringValue)}";
                    }
                }

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
