using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SportStorage.Models
{
    public class EfProductRepository: IProductRepository
    {
        private readonly ApplicationDbContext _ctx;

        public EfProductRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<Product> Products => _ctx.Products;
        public void Save(Product product)
        {
            if (product.ProductId == 0)
            {
                _ctx.Products.Add(product);
            }
            else
            {
                Product item = _ctx.Products.FirstOrDefault(m => m.ProductId == product.ProductId);
                if (item != null)
                {
                    item.Name = product.Name;
                    item.Description = product.Description;
                    item.Price = product.Price;
                    item.Category = product.Category;
                }
            }

            _ctx.SaveChanges();
        }

        public Product Delete(int productId)
        {
            Product product = _ctx.Products.FirstOrDefault(m => m.ProductId == productId);
            if (product != null)
            {
                _ctx.Products.Remove(product);
                _ctx.SaveChanges();
            }

            return product;
        }
    }
}