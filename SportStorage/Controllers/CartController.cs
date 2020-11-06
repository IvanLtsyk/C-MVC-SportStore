using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SportStorage.Models;
using SportStorage.Infrastructure;
using SportStorage.Models.ViewModels;

namespace SportStorage.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly Cart _cart;

        public CartController( IProductRepository productRepository, Cart cart)
        {
            _productRepository = productRepository;
            _cart = cart;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = _cart,
                ReturnUrl = returnUrl
            });
        }
        
        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = _productRepository.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                _cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = _productRepository.Products.FirstOrDefault(m => m.ProductId == productId);
            if (product != null)
            {
                _cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new {returnUrl});
        }
    }
}