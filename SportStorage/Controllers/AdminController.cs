using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportStorage.Models;

namespace SportStorage.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;

        public AdminController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        // GET
        public IActionResult Index()
        {
            return View(_productRepository.Products);
        }

        public ViewResult Edit(int productId)
        {
            return View(_productRepository.Products.FirstOrDefault(m => m.ProductId == productId));
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Save(product);
                TempData["message"] = $"{product.Name} has ben saved";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ViewResult Create() => View("Edit", new Product());

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            Product deleteProduct = _productRepository.Delete(productId);
            if (deleteProduct != null)
            {
                TempData["message"] = $"{deleteProduct.Name} was deleted";
            }

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public IActionResult SeedDatabase()
        {
            SeedData.EnsurePopulated(HttpContext.RequestServices);

            return RedirectToAction(nameof(Index));
        }
    }
}