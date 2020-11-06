using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportStorage.Models;
using SportStorage.Models.ViewModels;

namespace SportStorage.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ViewResult List(string category, int productPage = 1)
        {
            return View(new ProductsListViewModel
                {
                    Products = _productRepository.Products
                        .Where(m => category == null || m.Category == category)
                        .OrderBy(m => m.ProductId)
                        .Skip((productPage - 1) * PageSize)
                        .Take(PageSize),
                    PaginationInfo = new PaginationInfo
                    {
                        CurrentPage = productPage,
                        ItemsPerPages = PageSize,
                        TotalItems = category == null
                            ? _productRepository.Products.Count()
                            : _productRepository.Products.Count(m => m.Category == category)
                    },
                    CurrentCategory = category,
                }
            );
        }
    }
}