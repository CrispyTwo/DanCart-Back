using DanCart.WebApi.Areas.Products.DTOs;

namespace DanCart.WebApi.Areas.Products.Services.IServices;

public interface IProductsBlobService
{
    ProductDTO AttachImages(ProductDTO product);
    public IEnumerable<ProductDTO> AttachImages(IEnumerable<ProductDTO> products);
    public Task UploadMainImageAsync(int productId, Stream imageStream, string contentType);
}
