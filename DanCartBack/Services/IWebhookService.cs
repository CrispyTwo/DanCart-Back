namespace ECommerceAdmin.Services
{
    public interface IWebhookService
    {
        Task TriggerWebhookAsync(string eventType, object data);
        Task RegisterWebhookAsync(string url, string[] events);
        Task UnregisterWebhookAsync(string url);
    }
}
