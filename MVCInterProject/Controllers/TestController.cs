using Microsoft.AspNetCore.Mvc;

namespace MVCInterProject.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<List<int>> Something()
        {
            return new List<List<int>> { new List<int>{ 1 }, new List<int>{ 34 }, new List<int>{ 4, 5, 5 } };
        }
    }
}