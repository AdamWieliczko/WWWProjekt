#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCInterProject.Models;

namespace MVCInterProject.Controllers
{
    public class PackagesController : Controller
    {
        private readonly AW_TestContext _context;

        public PackagesController(AW_TestContext context)
        {
            _context = context;
        }


        [BasicAuthentication()]
        public async Task<IActionResult> Index(string filterBy = "", int currentPage = 0)
        {
            var packages = _context.Packages;
            var howManyForPage = 10;
            var howManyToSkip = currentPage * howManyForPage;

            ViewData["CurrentPage"] = currentPage;
            ViewData["Filter"] = filterBy;

            if (currentPage < 0)
            {
                return Redirect("./?filterBy=" + filterBy);
            }

            IQueryable<Package> packagesList;

            switch (filterBy)
            {
                case "OnlyOpen":
                    packagesList = packages.Where(i => i.PacIsOpen);
                    break;
                case "OnlyClosed":
                    packagesList = packages.Where(i => !i.PacIsOpen);
                    break;
                default:
                    packagesList = packages;
                    break;
            }

            var howManyPackages = packagesList.Count();

            var howManyPages = Math.Ceiling((decimal)howManyPackages / howManyForPage);

            if (currentPage >= howManyPages)
            {
                return Redirect("Packages/?filterBy=" + filterBy + "&currentPage=" + (howManyPages - 1)); //przekazywany typ filtrowania i ostatnia strona
            }

            ViewData["HowManyPages"] = howManyPages;

            return View(packagesList.Skip(howManyToSkip).Take(howManyForPage).ToList().OrderBy(model => model.PacId));
        }


        [BasicAuthentication()]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Packages
                .FirstOrDefaultAsync(m => m.PacId == id);
            if (package == null)
            {
                return NotFound();
            }

            package.Shippings = await _context.Shippings.Where(s => s.ShiPackage == package).ToListAsync();

            return View(package);
        }

        [BasicAuthentication()]
        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            if (id == null)
            {
                var pac = new Package();
                pac.PacCreateDate = DateTime.Now;
                pac.Shippings = new List<Shipping>();
                return View("CreateOrEdit", pac);
            }

            var package = await _context.Packages.FindAsync(id);

            if (package == null)
            {
                return NotFound();
            }

            package.Shippings = await _context.Shippings.Where(s => s.ShiPackage == package).ToListAsync();

            return View("CreateOrEdit", package);
        }
        
        [BasicAuthentication()]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShippingToPackage([Bind("PacId,PacName,PacCreateDate,PacIsOpen,PacCloseDate,PacCity,Shippings,ToBeSaved,IsInDb")] Package package)
        {
            var shipping = new Shipping();
            shipping.IsInDb = false;
            if(package.Shippings == null)
            {
                package.Shippings = new List<Shipping>();
            }
            shipping.ShiCreateDate = DateTime.Now;
            package.Shippings.Add(shipping);
            return PartialView("RefreshShippings", package);
        }

        [BasicAuthentication()]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveShippingFromPackage([Bind("PacId,PacName,PacCreateDate,PacIsOpen,PacCloseDate,PacCity,Shippings,ToBeSaved,IsInDb")] Package package, int index)
        {
            package.Shippings.RemoveAt(index);
            return PartialView("RefreshShippings", package);
        }

        [BasicAuthentication()]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("PacId,PacName,PacCreateDate,PacIsOpen,PacCloseDate,PacCity,Shippings,ToBeSaved,IsInDb")] Package package)
        {
            if (package.PacIsOpen == true)
            {
                package.PacCloseDate = null;
            }
            else
            {
                package.PacCloseDate = DateTime.Now;
            }

            if (ModelState.IsValid && package.PacId != 0) //to edytuje
            {
                try
                {
                    foreach (var toBeDeleted in _context.Shippings.Where(i => !package.Shippings.Contains(i) && package.PacId == i.ShiPackageId))
                    {
                        _context.Remove(toBeDeleted);
                    }

                    var shippings = package.Shippings;

                    for(int i = 0; i < shippings.Count; i++)
                    {
                        var currentShipping = shippings.ElementAt(i);

                        if(currentShipping.IsInDb)
                        {
                            _context.Update(currentShipping);
                        }
                        else
                        {
                            _context.Add(currentShipping);
                        }
                    }
                    _context.Update(package);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageExists(package.PacId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid) //to tworzy nowe
            {
                package.PacCreateDate = DateTime.Now;
                _context.Add(package);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View("CreateOrEdit", package);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Packages
                .FirstOrDefaultAsync(m => m.PacId == id);
            if (package == null)
            {
                return NotFound();
            }

            package.Shippings = await _context.Shippings.Where(s => s.ShiPackage == package).ToListAsync();

            return View(package);
        }

        [BasicAuthentication()]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            _context.Packages.Remove(package);
            var shippings = _context.Shippings.Where(i => i.ShiPackageId.Equals(id));
            _context.Shippings.RemoveRange(shippings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool PackageExists(int id)
        {
            return _context.Packages.Any(e => e.PacId == id);
        }
    }
}
