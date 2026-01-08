using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DanCart.WebApi.Areas.Products.DTOs;
using DanCart.WebApi.Areas.Products.Services.IServices;

namespace DanCart.WebApi.Areas.Products.Services;

public class ProductsBlobService : IProductsBlobService
{
    private readonly BlobContainerClient _containerClient;
    private readonly string _imageBaseUrl;

    private readonly IMapper _mapper;
    public ProductsBlobService(IConfiguration config, IMapper mapper)
    {
        _mapper = mapper;

        var containerName = "product-images";
        var blobUrl = config["AzureBlobStorage:BaseUrl"] ?? throw new ArgumentNullException("AzureBlobStorage:BaseUrl");
        _imageBaseUrl = blobUrl.TrimEnd('/') + "/" + containerName;
        _containerClient = new BlobServiceClient(config["AzureBlobStorage:DefaultConnection"]).GetBlobContainerClient(containerName);
    }

    public IEnumerable<ProductDTO> AttachImages(IEnumerable<ProductDTO> products)
    {
        foreach (var product in products)
        {
            product.Images = [$"{_imageBaseUrl}/{product.Id}/01.jpg"];
        }

        return products;
    }

    public ProductDTO AttachImages(ProductDTO product)
    {
        product.Images = [$"{_imageBaseUrl}/{product.Id}/01.jpg"];
        return product;
    }

    public async Task UploadMainImageAsync(int productId, Stream imageStream, string contentType)
    {
        var blobName = $"{productId}/01.jpg";
        var blobClient = _containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = contentType });
    }
}