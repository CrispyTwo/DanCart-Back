using FluentResults;
using static DanCart.WebApi.Areas.Products.Services.ProductsVectorSearchService;

namespace DanCart.WebApi.Areas.Products.Services.IServices;

public interface IProductsVectorSearchService
{
    Task<Result> MergeOrCreateAsync(Guid productId);
    Task<Result> DeleteAsync(Guid productId);
    Task<Result<IEnumerable<SearchModel>>> GetRankingAsync(string search);
}
