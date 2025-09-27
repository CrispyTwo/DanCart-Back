using DanCart.Models.DTOs.Product;

namespace DanCart.DataAccess.Blob;

public interface IBlobService
{
    Task<ProductFileUploadResponse> GenerateSignedUriAsync(string containerName, string blobName, string contentType,
        TimeSpan? validFor = null, CancellationToken cancellationToken = default);
}
