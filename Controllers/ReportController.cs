using Microsoft.AspNetCore.Mvc;
using PlacementDashboard.Data;

namespace PlacementDashboard.Controllers
{
    /// <summary>
    /// Crystal Reports Controller
    /// Implements report structures matching Crystal Reports:
    /// - Report Header (College branding, title, generated timestamp)
    /// - Group Header & Footers (Grouped by Department / Company)
    /// - Details Section (Individual student placements)
    /// - Summary & Grand Totals (Placed count, average LPA, maximum LPA)
    /// - Professional Print & Export view (CSS print rules)
    /// </summary>
    public class ReportController : Controller
    {
        private readonly PlacementRepository _repo;

        public ReportController(PlacementRepository repo)
        {
            _repo = repo;
        }

        // GET: /Report/Index
        public IActionResult Index()
        {
            var stats = _repo.GetDashboardStats();
            ViewBag.Departments = _repo.GetDistinctDepartments();
            return View(stats);
        }

        // GET: /Report/PlacementReport (Crystal Report layout: Grouped by Department)
        public IActionResult PlacementReport(string? department, string? batch)
        {
            ViewBag.SelectedDepartment = department;
            ViewBag.SelectedBatch = batch;
            ViewBag.Departments = _repo.GetDistinctDepartments();

            var students = _repo.GetAllStudents(department, "Placed", null);
            if (!string.IsNullOrWhiteSpace(batch))
            {
                students = students.Where(s => s.BatchYear == batch).ToList();
            }

            ViewBag.TotalCount = students.Count;
            ViewBag.MaxPackage = students.Count > 0 ? students.Max(s => s.Package ?? 0) : 0;
            ViewBag.AvgPackage = students.Count > 0 ? Math.Round(students.Average(s => s.Package ?? 0), 2) : 0;

            return View(students);
        }

        // GET: /Report/CompanyWiseReport (Crystal Report layout: Grouped by Recruiting Company)
        public IActionResult CompanyWiseReport()
        {
            var companies = _repo.GetAllCompanies();
            var placedStudents = _repo.GetAllStudents(null, "Placed", null);

            ViewBag.Students = placedStudents;
            return View(companies);
        }
    }
}
