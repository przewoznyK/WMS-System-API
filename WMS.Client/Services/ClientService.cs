using MudBlazor;
using System.Text.Json;
using WMS.Client.Responses;

namespace WMS.Client.Services
{
    public class ClientService
    {
        private readonly HttpClient _http;
        private readonly ISnackbar _snackbar;
        private readonly ILogger<ClientService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ClientService(HttpClient http, ISnackbar snackbar, ILogger<ClientService> logger)
        {
            _http = http;
            _snackbar = snackbar;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<T>> GetListAsync<T>(string endpoint, CancellationToken ct = default)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<T>>(endpoint, ct);
                return result ?? new List<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Data load error from endpoint: {Endpoint}", endpoint);
                _snackbar.Add($"Data load error: {ex.Message}", Severity.Error);
                return new List<T>();
            }
        }

        public async Task<bool> PostAsync<TRequest>(string endpoint, TRequest request, CancellationToken ct = default)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(endpoint, request, ct);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorJson = await response.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("API returned an error from {Endpoint}. Raw Error: {ErrorJson}", endpoint, errorJson);

                try
                {
                    var apiError = JsonSerializer.Deserialize<ApiError>(errorJson, _jsonOptions);

                    string errorMessage = "Undefined server error.";

                    if (apiError?.Errors != null && apiError.Errors.Any())
                    {
                        var allErrors = apiError.Errors.SelectMany(e => e.Value);
                        errorMessage = string.Join(" ", allErrors);
                    }
                    else if (!string.IsNullOrEmpty(apiError?.Details))
                    {
                        errorMessage = apiError.Details;
                    }
                    else if (!string.IsNullOrEmpty(apiError?.Title))
                    {
                        errorMessage = apiError.Title;
                    }

                    _snackbar.Add(errorMessage, Severity.Error);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while parsing the API error response from {Endpoint}.", endpoint);
                    _snackbar.Add("An error occurred while connecting to the API.", Severity.Error);
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Network or server error during POST request to {Endpoint}.", endpoint);
                _snackbar.Add("Request failed. Please check your connection.", Severity.Error);
                return false;
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint, CancellationToken ct = default)
        {
            try
            {
                return await _http.GetFromJsonAsync<T>(endpoint, ct);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Resource not found at {Endpoint}", endpoint);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Data load error from endpoint: {Endpoint}", endpoint);
                _snackbar.Add($"Data load error: {ex.Message}", Severity.Error);
                return default;
            }
        }
    }
}
