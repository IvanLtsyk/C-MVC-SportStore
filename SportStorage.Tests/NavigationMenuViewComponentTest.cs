using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportStorage.Components;
using SportStorage.Models;
using Xunit;

namespace SportStorage.Tests
{
    public class NavigationMenuViewComponentTest
    {
        [Fact]
        public void Can_Select_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
                {
                    new Product {ProductId = 1, Name = "P1", Category = "C1"},
                    new Product {ProductId = 2, Name = "P2", Category = "C2"},
                    new Product {ProductId = 3, Name = "P3", Category = "C3"},
                    new Product {ProductId = 4, Name = "P4", Category = "C4"},
                    new Product {ProductId = 5, Name = "P5", Category = "C1"},
                }).AsQueryable()
            );
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };

            string categoryToSelect = "Apples";
            target.RouteData.Values["category"] = categoryToSelect;
            string result = (string) (target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];
            string[] results =
                ((IEnumerable<string>) (target.Invoke() as ViewComponentResult).ViewData.Model).ToArray();

            Assert.Equal(categoryToSelect, result);
            Assert.True(Enumerable.SequenceEqual(new string[] {"C1", "C2", "C3", "C4"}, results));
        }
    }
}