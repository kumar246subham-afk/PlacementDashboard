using Microsoft.AspNetCore.Mvc;
using PlacementDashboard.Data;
using PlacementDashboard.Models;

namespace PlacementDashboard.Controllers
{
    public class CompanyController : Controller
    {
        private readonly PlacementRepository _repo;

        public CompanyController(PlacementRepository repo)
        {
            _repo = repo;
        }

        // GET: /Company/Index
        public IActionResult Index()
        {
            var companies = _repo.GetAllCompanies();
            return View(companies);
        }

        // GET: /Company/Create
        public IActionResult Create()
        {
            return View(new Company { DriveDate = DateTime.Today, MinCGPA = 6.5m, PackageOffered = 8.0m });
        }

        // POST: /Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (!ModelState.IsValid) return View(company);

            try
            {
                int newId = _repo.AddCompany(company);
                TempData["Success"] = $"Company '{company.CompanyName}' registered for recruitment drive! (ID: {newId})";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error adding company: {ex.Message}");
                return View(company);
            }
        }

        // GET: /Company/Edit/5
        public IActionResult Edit(int id)
        {
            var company = _repo.GetCompanyById(id);
            if (company == null)
            {
                TempData["Error"] = "Company not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // POST: /Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Company company)
        {
            if (id != company.CompanyId) return BadRequest();
            if (!ModelState.IsValid) return View(company);

            try
            {
                bool ok = _repo.UpdateCompany(company);
                if (ok)
                {
                    TempData["Success"] = $"Company '{company.CompanyName}' updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Update failed.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating company: {ex.Message}");
                return View(company);
            }
        }

        // GET: /Company/Delete/5
        public IActionResult Delete(int id)
        {
            var company = _repo.GetCompanyById(id);
            if (company == null)
            {
                TempData["Error"] = "Company not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // POST: /Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var company = _repo.GetCompanyById(id);
                bool ok = _repo.DeleteCompany(id);
                if (ok) TempData["Success"] = $"Company '{company?.CompanyName}' removed!";
                else TempData["Error"] = "Delete failed.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting company: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
