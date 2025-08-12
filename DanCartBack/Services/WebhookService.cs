using System.Text;
using System.Text.Json;

namespace ECommerceAdmin.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WebhookService> _logger;
        private readonly List<WebhookEndpoint> _webhookEndpoints;

        public WebhookService(HttpClient httpClient, ILogger<WebhookService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _webhookEndpoints = new List<WebhookEndpoint>();
        }

        public async Task TriggerWebhookAsync(string eventType, object data)
        {
            var relevantEndpoints = _webhookEndpoints
                .Where(w => w.Events.Contains(eventType) || w.Events.Contains("*"))
                .ToList();

            var webhookPayload = new
            {
                eventType,
                data,
                timestamp = DateTime.UtcNow,
                id = Guid.NewGuid().ToString()
            };

            var json = JsonSerializer.Serialize(webhookPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var tasks = relevantEndpoints.Select(async endpoint =>
            {
                try
                {
                    var response = await _httpClient.PostAsync(endpoint.Url, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning($"Webhook failed for {endpoint.Url}: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error sending webhook to {endpoint.Url}");
                }
            });

            await Task.WhenAll(tasks);
        }

        public Task RegisterWebhookAsync(string url, string[] events)
        {
            var existing = _webhookEndpoints.FirstOrDefault(w => w.Url == url);
            if (existing != null)
            {
                existing.Events = events;
            }
            else
            {
                _webhookEndpoints.Add(new WebhookEndpoint { Url = url, Events = events });
            }

            return Task.CompletedTask;
        }

        public Task UnregisterWebhookAsync(string url)
        {
            var existing = _webhookEndpoints.FirstOrDefault(w => w.Url == url);
            if (existing != null)
            {
                _webhookEndpoints.Remove(existing);
            }

            return Task.CompletedTask;
        }

        private class WebhookEndpoint
        {
            public string Url { get; set; } = string.Empty;
            public string[] Events { get; set; } = Array.Empty<string>();
        }
    }
}
