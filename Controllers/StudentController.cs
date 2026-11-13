using Microsoft.AspNetCore.Mvc;
using PlacementDashboard.Data;
using PlacementDashboard.Models;

namespace PlacementDashboard.Controllers
{
    public class StudentController : Controller
    {
        private readonly PlacementRepository _repo;

        public StudentController(PlacementRepository repo)
        {
            _repo = repo;
        }

        // GET: /Student/Index (Demonstrates Data Control GridView Binding & ADO.NET Search)
        public IActionResult Index(string? department, string? status, string? search)
        {
            ViewBag.SelectedDepartment = department;
            ViewBag.SelectedStatus = status;
            ViewBag.Search = search;
            ViewBag.Departments = _repo.GetDistinctDepartments();

            var students = _repo.GetAllStudents(department, status, search);
            return View(students);
        }

        // GET: /Student/Details/5 (Demonstrates DetailsView Data Control equivalent)
        public IActionResult Details(int id)
        {
            var student = _repo.GetStudentById(id);
            if (student == null)
            {
                TempData["Error"] = $"Student with ID {id} not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: /Student/Create (Demonstrates FormView / Insert Control equivalent)
        public IActionResult Create()
        {
            ViewBag.Departments = GetDefaultDepartments();
            return View(new Student { BatchYear = "2024-2025", CGPA = 7.5m });
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = GetDefaultDepartments();
                return View(student);
            }

            try
            {
                int newId = _repo.AddStudent(student);
                TempData["Success"] = $"Student '{student.StudentName}' registered successfully! (ID: {newId})";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Database Error: {ex.Message}");
                ViewBag.Departments = GetDefaultDepartments();
                return View(student);
            }
        }

        // GET: /Student/Edit/5
        public IActionResult Edit(int id)
        {
            var student = _repo.GetStudentById(id);
            if (student == null)
            {
                TempData["Error"] = $"Student with ID {id} not found.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = GetDefaultDepartments();
            return View(student);
        }

        // POST: /Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.StudentId) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = GetDefaultDepartments();
                return View(student);
            }

            try
            {
                bool ok = _repo.UpdateStudent(student);
                if (ok)
                {
                    TempData["Success"] = $"Student '{student.StudentName}' details updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Update failed. Student record not found.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating student: {ex.Message}");
                ViewBag.Departments = GetDefaultDepartments();
                return View(student);
            }
        }

        // GET: /Student/Delete/5
        public IActionResult Delete(int id)
        {
            var student = _repo.GetStudentById(id);
            if (student == null)
            {
                TempData["Error"] = $"Student with ID {id} not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var student = _repo.GetStudentById(id);
                bool ok = _repo.DeleteStudent(id);
                if (ok)
                {
                    TempData["Success"] = $"Student '{student?.StudentName}' deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Delete operation failed.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        private List<string> GetDefaultDepartments()
        {
            var list = _repo.GetDistinctDepartments();
            var defaults = new List<string> { "Computer Engineering", "Information Technology", "Mechanical Engineering", "Electrical Engineering", "Civil Engineering" };
            foreach (var d in defaults)
            {
                if (!list.Contains(d)) list.Add(d);
            }
            return list;
        }
    }
}
