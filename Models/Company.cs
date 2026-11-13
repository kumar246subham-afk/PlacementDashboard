using System.ComponentModel.DataAnnotations;

namespace PlacementDashboard.Models
{
    public class Company
    {
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [StringLength(100)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Industry / Sector is required")]
        [StringLength(50)]
        public string Industry { get; set; } = string.Empty;

        [Display(Name = "Eligible Min CGPA")]
        [Range(0, 10)]
        public decimal MinCGPA { get; set; } = 6.0m;

        [Required]
        [Display(Name = "Offered Package (LPA ₹)")]
        [Range(0.1, 100)]
        public decimal PackageOffered { get; set; }

        [Display(Name = "Students Hired")]
        public int StudentsHired { get; set; }

        [Required]
        [Display(Name = "Drive Date")]
        [DataType(DataType.Date)]
        public DateTime DriveDate { get; set; } = DateTime.Today;

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(150)]
        [Url]
        public string? Website { get; set; }
    }
}
