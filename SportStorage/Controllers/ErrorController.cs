using Microsoft.AspNetCore.Mvc;

namespace SportStorage.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Error() => View();
    }
}