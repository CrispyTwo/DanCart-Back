namespace DanCart.Models.Auth;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Token { get; set; }
    public required string UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public DateTime ExpiresAt { get; set; }
}
