using System.ComponentModel.DataAnnotations;

namespace PlacementDashboard.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Student Name is required")]
        [StringLength(100)]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Roll Number is required")]
        [StringLength(20)]
        [Display(Name = "Roll Number")]
        public string RollNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50)]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "CGPA is required")]
        [Range(0.0, 10.0, ErrorMessage = "CGPA must be between 0 and 10")]
        public decimal CGPA { get; set; }

        [Required(ErrorMessage = "Batch Year is required")]
        [StringLength(10)]
        [Display(Name = "Batch Year")]
        public string BatchYear { get; set; } = "2024-2025";

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(15)]
        public string? Phone { get; set; }

        [Required]
        [Display(Name = "Placement Status")]
        public string PlacementStatus { get; set; } = "Unplaced"; // Placed, Unplaced, In-Process

        [Display(Name = "Placed Company")]
        [StringLength(100)]
        public string? CompanyName { get; set; }

        [Display(Name = "Package (LPA ₹)")]
        [Range(0, 100, ErrorMessage = "Package must be valid in LPA")]
        public decimal? Package { get; set; }

        [Display(Name = "Offer Date")]
        [DataType(DataType.Date)]
        public DateTime? OfferDate { get; set; }
    }
}
