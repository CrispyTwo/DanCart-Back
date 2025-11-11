using DanCart.Models.Products;

namespace DanCart.DataAccess.Repository.IRepository;
public interface IProductRepository : IRepository<Product>
{
    void Update(Product obj);
}