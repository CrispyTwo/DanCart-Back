namespace DanCart.DataAccess.Models;

public class FullTextResult<T>
{
    public T Entity { get; set; } = default!;
    public float Rank { get; set; }
}