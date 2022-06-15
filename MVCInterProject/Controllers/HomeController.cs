using Microsoft.AspNetCore.Mvc;
using MVCInterProject.Models;
using System.Diagnostics;

namespace MVCInterProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AW_TestContext _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = new AW_TestContext();
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sort()
        {
            return View();
        }

        public IActionResult Storage()
        {
            return View();
        }

        public async Task<IActionResult> Create([Bind("PerId,PerName,PerPassword,PerIsAdmin")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
            }
            
            return View("CorrectlyRegister");
        }

        public IActionResult SaveData()
        {
            return View();
        }

        public async Task<IActionResult> EditPackage(int packageId)
        {
            var package = await _context.Packages.FindAsync(packageId);
            return View(package);
        }
        
        public IActionResult Delete()
        {
            return View();
        }

        public String ShowList() //testy mechaniki obsługi Packagów
        {
            var db = new AW_TestContext();
            var shipping = new Shipping();

            var package = new Package();

            shipping.ShiCreateDate = DateTime.Now;

            package.PacName = "TestPackage";
            package.PacCity = "Cracow";
            package.PacCreateDate = DateTime.Now;
            package.PacIsOpen = true;
            package.Shippings = new List<Shipping>();// { shipping };
            var l = db.Add(package);
            db.SaveChanges();
            return "okej";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}