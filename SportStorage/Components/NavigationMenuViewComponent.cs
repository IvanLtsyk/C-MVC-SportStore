using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportStorage.Models;

namespace SportStorage.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IProductRepository _repository;

        public NavigationMenuViewComponent(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(_repository.Products
                .Select(m => m.Category)
                .Distinct()
                .OrderBy(m => m)
            );
        }
    }
}