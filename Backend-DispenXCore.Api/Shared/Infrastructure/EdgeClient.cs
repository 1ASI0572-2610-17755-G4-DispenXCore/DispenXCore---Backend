using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Backend_DispenXCore.Api.Shared.Infrastructure
{
    public class EdgeClient : IEdgeClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EdgeClient> _logger;
        private readonly string _baseUrl;

        public EdgeClient(HttpClient httpClient, IConfiguration configuration, ILogger<EdgeClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = configuration["EdgeServiceUrl"] ?? "http://localhost:5000";
        }

        public async Task<bool> ActivateDispenserAsync(string deviceId, string? supplyType)
        {
            var url = $"{_baseUrl.TrimEnd('/')}/api/v1/dispenser/activate";
            var payload = new
            {
                device_id = deviceId,
                supply_type = supplyType ?? "General"
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                _logger.LogInformation("[EDGE-CLIENT] Sending dispense command to {Url} with payload {Payload}...", url, json);
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("[EDGE-CLIENT] Dispense command succeeded with status {StatusCode}", response.StatusCode);
                    return true;
                }
                else
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("[EDGE-CLIENT] Edge service returned status {StatusCode}: {Error}", response.StatusCode, errorText);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EDGE-CLIENT] Error sending dispense command to Edge service");
            }

            return false;
        }
    }
}
