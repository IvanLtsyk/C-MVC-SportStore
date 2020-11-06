using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStorage.Controllers;
using SportStorage.Models;
using Xunit;

namespace SportStorage.Tests
{
    public class OrderControllerTests
    {
        [Fact] // перевірка можливості переходу до оплати при пустій корзині
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderRepository> repo = new Mock<IOrderRepository>(); // фейковий репозиторій
            Cart cart = new Cart(); // корзина
            
            Order order = new Order();
            OrderController target = new OrderController(repo.Object, cart);

            ViewResult result = target.Checkout(order) as ViewResult;
            // перевірка, що замовлення не було збережене.
            repo.Verify(m=>m.Save(It.IsAny<Order>()), Times.Never()); 
            // перевірка, що вертається стандартне представлення.
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //перевірка, що передставленню передана не валідна модель
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]// перевірка можливості переходу до оплати при не валідній моделі
        public void Cannot_Checkout_Invalids_ShippingDetails()
        {
            Mock<IOrderRepository> repo = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            
            OrderController target = new OrderController(repo.Object, cart);
            
            target.ModelState.AddModelError("test error", "test error");
            
            ViewResult result = target.Checkout(new Order()) as ViewResult;
            
            repo.Verify(m=>m.Save(It.IsAny<Order>()), Times.Never);
            // перевірка, що вертається стандартне представлення.
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //перевірка, що передставленню передана не валідна модель
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cna_Checkout_And_Submit_Orders()
        {
            Mock<IOrderRepository> repo = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController target = new OrderController(repo.Object, cart);
            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            repo.Verify(m => m.Save(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result.ActionName);
        }
    }
}