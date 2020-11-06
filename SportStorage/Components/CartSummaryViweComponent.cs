using Microsoft.AspNetCore.Mvc;
using SportStorage.Models;

namespace SportStorage.Components
{
    public class CartSummaryViewComponent: ViewComponent
    {
        private readonly Cart _cartServices;

        public CartSummaryViewComponent(Cart cartServices)
        {
            _cartServices = cartServices;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cartServices);
        }
    }
}