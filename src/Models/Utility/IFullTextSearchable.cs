using NpgsqlTypes;

namespace DanCart.Models.Utility;

public interface IFullTextSearchable
{
    public NpgsqlTsVector SearchVector { get; set; }
}
