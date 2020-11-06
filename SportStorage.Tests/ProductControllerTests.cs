using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStorage.Controllers;
using SportStorage.Models;
using SportStorage.Models.ViewModels;
using Xunit;

namespace SportStorage.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "C1"},
                new Product {ProductId = 2, Name = "P2", Category = "C2"},
                new Product {ProductId = 3, Name = "P3", Category = "C3"},
                new Product {ProductId = 4, Name = "P4", Category = "C4"},
                new Product {ProductId = 5, Name = "P5", Category = "C1"},
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object) {PageSize = 3};

            ProductsListViewModel result = controller.List("C1", 1).ViewData.Model as ProductsListViewModel;

            PaginationInfo pageInfo = result.PaginationInfo;
            Assert.Equal(1, pageInfo.CurrentPage);
            Assert.Equal(2, pageInfo.ItemsPerPages);
            Assert.Equal(2, pageInfo.TotalItems);
            Assert.Equal(1, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "C1"},
                new Product {ProductId = 2, Name = "P2", Category = "C2"},
                new Product {ProductId = 3, Name = "P3", Category = "C3"},
                new Product {ProductId = 4, Name = "P4", Category = "C4"},
                new Product {ProductId = 5, Name = "P5", Category = "C1"},
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object) {PageSize = 3};

            ProductsListViewModel result = controller.List("C1", 1).ViewData.Model as ProductsListViewModel;

            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.True(prodArray[0].Name == "P1" && prodArray[0].Category == "C1");
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "C1"},
                new Product {ProductId = 2, Name = "P2", Category = "C2"},
                new Product {ProductId = 3, Name = "P3", Category = "C3"},
                new Product {ProductId = 4, Name = "P4", Category = "C1"},
                new Product {ProductId = 5, Name = "P5", Category = "C5"},
            }).AsQueryable());
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            
            Func<ViewResult, ProductsListViewModel> GetModel = result => 
                result?.ViewData.Model as ProductsListViewModel;
            int? res1 = GetModel(target.List("C1"))?.PaginationInfo.TotalItems;
            int? res2 = GetModel(target.List("C2"))?.PaginationInfo.TotalItems;
            int? resAll = GetModel(target.List(null))?.PaginationInfo.TotalItems;

            Assert.Equal(2, res1);
            Assert.Equal(1, res2);
            Assert.Equal(5, resAll);
        }
    }
}