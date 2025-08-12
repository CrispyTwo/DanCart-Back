using ECommerceAdmin.Models;
using ECommerceAdmin.DTOs;

namespace ECommerceAdmin.Services
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreDto>> GetUserStoresAsync(string userId);
        Task<StoreDto?> GetStoreAsync(int storeId, string userId);
        Task<StoreDto> CreateStoreAsync(CreateStoreDto model, string userId);
        Task<StoreDto?> UpdateStoreAsync(int storeId, UpdateStoreDto model, string userId);
        Task<bool> DeleteStoreAsync(int storeId, string userId);
        Task<string> UploadLogoAsync(int storeId, IFormFile file, string userId);
        Task<StoreAnalyticsDto> GetStoreAnalyticsAsync(int storeId, string userId);
    }
}
