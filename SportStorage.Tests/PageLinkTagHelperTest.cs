using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportStorage.Infrastructure;
using SportStorage.Models.ViewModels;
using Xunit;

namespace SportStorage.Tests
{
    public class PageLinkTagHelperTest
    {
        [Fact]
        public void Can_Generate_Page_Links()
        {
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupSequence(m => m.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");
            
            var urlHelperFactory = new Mock<UrlHelperFactory>();
            urlHelperFactory.Setup(m => m.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(urlHelper.Object);
            
            PageLinkTagHelper helper = new PageLinkTagHelper(urlHelperFactory.Object)
            {
                PageModel = new PaginationInfo
                {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPages = 10
                },
                PageAction = "Test"
            };
            TagHelperContext ctx = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "" 
            );

            var content = new Mock<TagHelperContent>();
            TagHelperOutput output = new TagHelperOutput(
                "div",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(content.Object)
                );
            helper.Process(ctx, output);
            Assert.Equal(
                @"<a href=""Test/Pagel"">1</a>"
                +@"<a href=""Test/Pagel"">1</a>"
                +@"<a href=""Test/Pagel"">1</a>", output.Content.GetContent());
        }
    }
}