using System.Linq;

namespace SportStorage.Models
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        void Save(Product product);
        Product Delete(int productId);
    }
}