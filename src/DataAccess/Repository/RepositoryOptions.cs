using System.Linq.Expressions;

namespace DanCart.DataAccess.Repository;

public class GetRangeOptions<T>
{
    public Expression<Func<T, bool>>? Filter { get; set; }
    public IEnumerable<(string Name, bool Desc)> Sortings { get; set; } = [];
    public IEnumerable<string> IncludeProperties { get; set; } = [];
}
