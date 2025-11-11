using DanCart.DataAccess.Models;

namespace DanCart.DataAccess.Blob;

public interface IBlobService
{
    Task<FileUploadResponse> GenerateSignedUriAsync(string containerName, string blobName, string? contentType = null,
        TimeSpan? validFor = null, CancellationToken cancellationToken = default);
}
