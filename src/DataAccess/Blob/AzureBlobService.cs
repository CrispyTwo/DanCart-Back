using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using DanCart.Models.DTOs.Product;
using Microsoft.Extensions.Configuration;
namespace DanCart.DataAccess.Blob;

public class AzureBlobService(IConfiguration config) : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient = new(config["AzureBlobStorage:DefaultConnection"]);
    private readonly string _accountName = config["AzureBlobStorage:AccountName"] ?? throw new ArgumentNullException("AzureBlobStorage:AccountName");
    private readonly string _accountKey = config["AzureBlobStorage:AccountKey"] ?? throw new ArgumentNullException("AzureBlobStorage:AccountKey");

    public async Task<ProductFileUploadResponse> GenerateSignedUriAsync(string containerName, string blobName, string? contentType = null, 
        TimeSpan? validFor = null, CancellationToken cancellationToken = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        var blobClient = container.GetBlobClient(blobName);

        validFor ??= TimeSpan.FromMinutes(15);
        var expiresOn = DateTimeOffset.UtcNow.Add(validFor.Value);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = container.Name,
            BlobName = blobName,
            Resource = "b",
            ExpiresOn = expiresOn
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Create | BlobSasPermissions.Write);

        var credential = new StorageSharedKeyCredential(_accountName, _accountKey);
        var sasQuery = sasBuilder.ToSasQueryParameters(credential).ToString();
        var uploadUri = new Uri($"{blobClient.Uri}?{sasQuery}");

        return new() { Uri = uploadUri, ExpiresOn = expiresOn };
    }
}
