using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportStorage.Models;

namespace SportStorage.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Cart _cart;

        public OrderController(IOrderRepository orderRepository, Cart cart)
        {
            _orderRepository = orderRepository;
            _cart = cart;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (_cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                _orderRepository.Save(order);

                return RedirectToAction(nameof(Completed));
            }

            return View(order);
        }

        public ViewResult Completed()
        {
            _cart.Clear();
            return View();
        }

        [Authorize]
        public ViewResult List()
        {
            var filteredOrders = _orderRepository.Orders.Where(m => !m.Shiped);
            return View(filteredOrders);
        }

        [Authorize]
        [HttpPost]
        public IActionResult MarkShipped(int orderId)
        {
            Order order = _orderRepository.Orders.FirstOrDefault(m => m.OrderId == orderId);

            if (order != null)
            {
                order.Shiped = true;
                _orderRepository.Save(order);
            }

            return RedirectToAction(nameof(List));
        }
    }
}