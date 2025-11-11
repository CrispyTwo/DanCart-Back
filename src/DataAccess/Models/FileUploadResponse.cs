namespace DanCart.DataAccess.Models;

public class FileUploadResponse
{
    public Uri Uri { get; set; }
    public DateTimeOffset ExpiresOn { get; set; }
}
