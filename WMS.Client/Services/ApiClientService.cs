using MudBlazor;
using System.Net.Http.Headers;
using System.Text.Json;
using WMS.Client.Responses;
using WMS.Client.Services.Interfaces;

namespace WMS.Client.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ISnackbar _snackbar;
        private readonly ILogger<ApiClientService> _logger;
        private readonly ITokenStorageService _tokenStorageService;

        public ApiClientService(HttpClient http, ISnackbar snackbar, ILogger<ApiClientService> logger, ITokenStorageService tokenStorageService)
        {
            _http = http;
            _snackbar = snackbar;
            _logger = logger;
            _tokenStorageService = tokenStorageService;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T?> GetAsync<T>(string endpoint, CancellationToken ct = default)
        {
            var response = await SendGetAsync(endpoint, ct);

            if (response is null)
            {
                return default;
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>(_jsonOptions, ct);
            }

            await HandleErrorAsync(response, endpoint, ct);
            return default;
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(string endpoint, CancellationToken ct = default)
        {
            var response = await SendGetAsync(endpoint, ct);

            if (response is null)
            {
                return Enumerable.Empty<T>();
            }


            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<T>>( _jsonOptions, ct) ?? Enumerable.Empty<T>();
            }

            await HandleErrorAsync(
                response,
                endpoint,
                ct);

            return Enumerable.Empty<T>();
        }

        public async Task<bool> PostAsync<TRequest>(string endpoint, TRequest request, CancellationToken ct = default)
        {
            var response = await SendPostAsync(endpoint, request, ct);

            if (response is null)
            {
                return false;
            }

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            await HandleErrorAsync(response, endpoint, ct);
            return false;
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken ct = default)
        {
            var response = await SendPostAsync(endpoint, request, ct);

            if (response is null)
            {
                return default;
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, ct);
            }

            await HandleErrorAsync(response, endpoint, ct);

            return default;
        }

        private async Task<HttpResponseMessage?> SendGetAsync(string endpoint, CancellationToken ct)
        {
            await AttachTokenAsync();

            try
            {
                return await _http.GetAsync(endpoint, ct);
            }
            catch (Exception ex)
            {
                HandleException(ex, endpoint, "GET");
                return null;
            }
        }

        private async Task<HttpResponseMessage?> SendPostAsync<TRequest>(string endpoint, TRequest request, CancellationToken ct)
        {
            await AttachTokenAsync();

            try
            {
                return await _http.PostAsJsonAsync(endpoint, request, ct);
            }
            catch (Exception ex)
            {
                HandleException(ex, endpoint, "POST");
                return null;
            }
        }

        private async Task AttachTokenAsync()
        {
            var token = await _tokenStorageService.GetTokenAsync();
            _http.DefaultRequestHeaders.Authorization =string.IsNullOrWhiteSpace(token) ? null : new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task HandleErrorAsync(HttpResponseMessage response, string endpoint, CancellationToken ct)
        {

            var errorJson = await response.Content.ReadAsStringAsync(ct);
            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine($"Body: '{errorJson}'");
            _logger.LogWarning("API error {StatusCode} from {Endpoint}: {Error}", response.StatusCode, endpoint, errorJson);

            try
            {
                var apiError = JsonSerializer.Deserialize<ApiError>(errorJson, _jsonOptions);
                string message = $"Server error ({response.StatusCode})";

                if (apiError?.Errors?.Any() == true)
                {
                    message = string.Join(" ", apiError.Errors.SelectMany(x => x.Value));
                }
                else if (!string.IsNullOrWhiteSpace(apiError?.Details))
                {
                    message = apiError.Details;
                }
                else if (!string.IsNullOrWhiteSpace(apiError?.Title))
                {
                    message = apiError.Title;
                }

                _snackbar.Add(message, Severity.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot parse API error");
                _snackbar.Add("Unexpected server error.", Severity.Error);
            }
        }

        private void HandleException(Exception ex, string endpoint, string method)
        {
            _logger.LogError(ex, "{Method} request failed: {Endpoint}", method, endpoint);
            _snackbar.Add("Request failed. Check your connection.", Severity.Error);
        }
    }
}