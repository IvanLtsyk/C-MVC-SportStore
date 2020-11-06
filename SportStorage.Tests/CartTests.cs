using System.Collections.Generic;
using System.Linq;
using SportStorage.Models;
using Xunit;

namespace SportStorage.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            var product1 = new Product {ProductId = 1, Name = "P1"};
            var product2 = new Product {ProductId = 2, Name = "P2"};

            Cart target = new Cart();
            target.AddItem(product1, 1);
            target.AddItem(product2, 1);

            CartLine[] results = target.Lines.ToArray();
            Assert.Equal(2, results.Length);
            Assert.Equal(product1, results[0].Product);
            Assert.Equal(product2, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            var product1 = new Product {ProductId = 1, Name = "P1"};
            var product2 = new Product {ProductId = 2, Name = "P2"};

            Cart target = new Cart();
            target.AddItem(product1, 1);
            target.AddItem(product1, 10);
            target.AddItem(product2, 1);

            CartLine[] results = target.Lines
                .OrderBy(m => m.Product.ProductId).ToArray();
            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            var product1 = new Product {ProductId = 1, Name = "P1"};
            var product2 = new Product {ProductId = 2, Name = "P2"};
            var product3 = new Product {ProductId = 3, Name = "P3"};

            Cart target = new Cart();
            target.AddItem(product1, 1);
            target.AddItem(product2, 3);
            target.AddItem(product2, 1);
            target.AddItem(product3, 5);

            target.RemoveLine(product2);

            CartLine[] results = target.Lines
                .OrderBy(m => m.Product.ProductId).ToArray();

            Assert.Empty(target.Lines.Where(m => m.Product == product2));
            Assert.Equal(2, target.Lines.Count());
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            var product1 = new Product {ProductId = 1, Name = "P1", Price = 100M};
            var product2 = new Product {ProductId = 2, Name = "P2", Price = 50M};

            Cart target = new Cart();
            target.AddItem(product1, 1);
            target.AddItem(product2, 1);
            target.AddItem(product1, 3);

            decimal result = target.ComputeTotalValue();

            Assert.Equal(450M, result);
        }

        [Fact]
        public void Calculate_Clear_Contents()
        {
            var product1 = new Product {ProductId = 1, Name = "P1", Price = 100M};
            var product2 = new Product {ProductId = 2, Name = "P2", Price = 50M};

            Cart target = new Cart();
            target.AddItem(product1, 1);
            target.AddItem(product2, 1);
            target.AddItem(product1, 3);

            target.Clear();

            Assert.Empty(target.Lines);
        }
    }
}