using Microsoft.AspNetCore.Mvc;
using PlacementDashboard.Data;

namespace PlacementDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly PlacementRepository _repo;

        public DashboardController(PlacementRepository repo)
        {
            _repo = repo;
        }

        // GET: /Dashboard/Index
        public IActionResult Index()
        {
            var stats = _repo.GetDashboardStats();
            return View(stats);
        }
    }
}
