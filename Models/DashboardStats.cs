namespace PlacementDashboard.Models
{
    public class DashboardStats
    {
        public int TotalStudents { get; set; }
        public int PlacedStudents { get; set; }
        public int UnplacedStudents { get; set; }
        public decimal PlacementPercentage => TotalStudents > 0 ? Math.Round((decimal)PlacedStudents / TotalStudents * 100, 2) : 0;
        public decimal HighestPackage { get; set; }
        public decimal AveragePackage { get; set; }
        public int TotalCompanies { get; set; }
        public List<DepartmentStat> DepartmentBreakdown { get; set; } = new();
        public List<Student> RecentPlacements { get; set; } = new();
        public List<Company> TopRecruiters { get; set; } = new();
    }

    public class DepartmentStat
    {
        public string Department { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int PlacedCount { get; set; }
        public decimal PlacementRate => TotalStudents > 0 ? Math.Round((decimal)PlacedCount / TotalStudents * 100, 1) : 0;
        public decimal AvgPackage { get; set; }
    }
}
