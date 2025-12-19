using DanCart.WebApi.Areas.Products.DTOs;

namespace DanCart.WebApi.Areas.Products.Services.IServices;

public interface IProductsBlobService
{
    public IEnumerable<ProductDTO> AttachImages(IEnumerable<ProductDTO> products);
    public ProductWithImagesDTO AttachImage(ProductWithImagesDTO product);
    public Task UploadMainImageAsync(int productId, Stream imageStream, string contentType);
}
