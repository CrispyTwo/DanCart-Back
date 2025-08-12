using Microsoft.EntityFrameworkCore;
using ECommerceAdmin.Data;
using ECommerceAdmin.Models;
using ECommerceAdmin.DTOs;
using ECommerceAdmin.Services;

namespace ECommerceAdmin.Services
{
    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileUploadService _fileUploadService;
        private readonly IWebhookService _webhookService;

        public StoreService(
            ApplicationDbContext context,
            IFileUploadService fileUploadService,
            IWebhookService webhookService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _webhookService = webhookService;
        }

        public async Task<IEnumerable<StoreDto>> GetUserStoresAsync(string userId)
        {
            var stores = await _context.Stores
                .Where(s => s.UserId == userId)
                .Select(s => new StoreDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Domain = s.Domain,
                    LogoUrl = s.LogoUrl,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt,
                    ProductCount = s.Products.Count,
                    OrderCount = s.Orders.Count,
                    CustomerCount = s.Customers.Count
                })
                .ToListAsync();

            return stores;
        }

        public async Task<StoreDto?> GetStoreAsync(int storeId, string userId)
        {
            var store = await _context.Stores
                .Include(s => s.Settings)
                .Where(s => s.Id == storeId && s.UserId == userId)
                .Select(s => new StoreDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Domain = s.Domain,
                    LogoUrl = s.LogoUrl,
                    HeroBannerUrl = s.HeroBannerUrl,
                    FaviconUrl = s.FaviconUrl,
                    PrimaryColor = s.PrimaryColor,
                    SecondaryColor = s.SecondaryColor,
                    AccentColor = s.AccentColor,
                    FontFamily = s.FontFamily,
                    Template = s.Template,
                    Currency = s.Currency,
                    Timezone = s.Timezone,
                    TaxRate = s.TaxRate,
                    Address = s.Address,
                    Phone = s.Phone,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    ProductCount = s.Products.Count,
                    OrderCount = s.Orders.Count,
                    CustomerCount = s.Customers.Count
                })
                .FirstOrDefaultAsync();

            return store;
        }

        public async Task<StoreDto> CreateStoreAsync(CreateStoreDto model, string userId)
        {
            var store = new Store
            {
                Name = model.Name,
                Description = model.Description,
                Domain = GenerateUniqueDomain(model.Name),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            // Create default settings
            var settings = new StoreSettings
            {
                StoreId = store.Id
            };
            _context.StoreSettings.Add(settings);
            await _context.SaveChangesAsync();

            // Trigger webhook
            await _webhookService.TriggerWebhookAsync("store.created", new { storeId = store.Id, userId });

            return new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                Domain = store.Domain,
                IsActive = store.IsActive,
                CreatedAt = store.CreatedAt,
                ProductCount = 0,
                OrderCount = 0,
                CustomerCount = 0
            };
        }

        public async Task<StoreDto?> UpdateStoreAsync(int storeId, UpdateStoreDto model, string userId)
        {
            var store = await _context.Stores
                .FirstOrDefaultAsync(s => s.Id == storeId && s.UserId == userId);

            if (store == null)
                return null;

            store.Name = model.Name;
            store.Description = model.Description;
            store.PrimaryColor = model.PrimaryColor;
            store.SecondaryColor = model.SecondaryColor;
            store.AccentColor = model.AccentColor;
            store.FontFamily = model.FontFamily;
            store.Template = model.Template;
            store.Currency = model.Currency;
            store.Timezone = model.Timezone;
            store.TaxRate = model.TaxRate;
            store.Address = model.Address;
            store.Phone = model.Phone;
            store.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Trigger webhook
            await _webhookService.TriggerWebhookAsync("store.updated", new { storeId = store.Id, userId });

            return await GetStoreAsync(storeId, userId);
        }

        public async Task<bool> DeleteStoreAsync(int storeId, string userId)
        {
            var store = await _context.Stores
                .FirstOrDefaultAsync(s => s.Id == storeId && s.UserId == userId);

            if (store == null)
                return false;

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            // Trigger webhook
            await _webhookService.TriggerWebhookAsync("store.deleted", new { storeId, userId });

            return true;
        }

        public async Task<string> UploadLogoAsync(int storeId, IFormFile file, string userId)
        {
            var store = await _context.Stores
                .FirstOrDefaultAsync(s => s.Id == storeId && s.UserId == userId);

            if (store == null)
                return string.Empty;

            var logoUrl = await _fileUploadService.UploadFileAsync(file, "logos");
            if (!string.IsNullOrEmpty(logoUrl))
            {
                store.LogoUrl = logoUrl;
                store.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return logoUrl;
        }

        public async Task<StoreAnalyticsDto> GetStoreAnalyticsAsync(int storeId, string userId)
        {
            var store = await _context.Stores
                .Include(s => s.Orders)
                .Include(s => s.Products)
                .Include(s => s.Customers)
                .FirstOrDefaultAsync(s => s.Id == storeId && s.UserId == userId);

            if (store == null)
                return new StoreAnalyticsDto();

            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var recentOrders = store.Orders.Where(o => o.CreatedAt >= thirtyDaysAgo).ToList();

            return new StoreAnalyticsDto
            {
                TotalRevenue = store.Orders.Where(o => o.Status == "fulfilled").Sum(o => o.Total),
                TotalOrders = store.Orders.Count,
                TotalCustomers = store.Customers.Count,
                TotalProducts = store.Products.Count,
                RecentRevenue = recentOrders.Where(o => o.Status == "fulfilled").Sum(o => o.Total),
                RecentOrders = recentOrders.Count,
                ActiveProducts = store.Products.Count(p => p.IsActive),
                LowStockProducts = store.Products.Count(p => p.Stock <= p.LowStockThreshold),
                OutOfStockProducts = store.Products.Count(p => p.Stock <= 0)
            };
        }

        private string GenerateUniqueDomain(string storeName)
        {
            var baseDomain = storeName.ToLower()
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace("\"", "");

            var domain = baseDomain;
            var counter = 1;

            while (_context.Stores.Any(s => s.Domain == domain))
            {
                domain = $"{baseDomain}-{counter}";
                counter++;
            }

            return domain;
        }
    }
}
