using System.Data;
using System.Data.SqlClient;
using PlacementDashboard.Models;

namespace PlacementDashboard.Data
{
    /// <summary>
    /// Data Access Layer using ADO.NET for Placement Dashboard and Crystal Reports.
    /// Demonstrates:
    /// 1. SqlConnection & Connection Management
    /// 2. SqlCommand with SqlParameter (Safe SQL)
    /// 3. SqlDataReader (Fast connected read)
    /// 4. SqlDataAdapter & DataTable (Disconnected architecture for Data Controls)
    /// 5. ExecuteScalar & ExecuteNonQuery
    /// </summary>
    public class PlacementRepository
    {
        private readonly string _connectionString;

        public PlacementRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PlacementDB")
                ?? "Server=(localdb)\\mssqllocaldb;Database=PlacementDB;Trusted_Connection=True;";
        }

        // ==================== STUDENTS CRUD ====================

        public List<Student> GetAllStudents(string? department = null, string? status = null, string? search = null)
        {
            var list = new List<Student>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = @"SELECT * FROM Students WHERE 1=1 ";
                if (!string.IsNullOrWhiteSpace(department)) query += " AND Department = @Department ";
                if (!string.IsNullOrWhiteSpace(status)) query += " AND PlacementStatus = @Status ";
                if (!string.IsNullOrWhiteSpace(search)) query += " AND (StudentName LIKE @Search OR RollNumber LIKE @Search OR CompanyName LIKE @Search) ";
                query += " ORDER BY RollNumber ASC";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(department)) cmd.Parameters.AddWithValue("@Department", department);
                    if (!string.IsNullOrWhiteSpace(status)) cmd.Parameters.AddWithValue("@Status", status);
                    if (!string.IsNullOrWhiteSpace(search)) cmd.Parameters.AddWithValue("@Search", $"%{search}%");

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapStudent(reader));
                        }
                    }
                }
            }
            return list;
        }

        public Student? GetStudentById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Students WHERE StudentId = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapStudent(reader);
                        }
                    }
                }
            }
            return null;
        }

        public int AddStudent(Student student)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Students 
                    (StudentName, RollNumber, Department, CGPA, BatchYear, Email, Phone, PlacementStatus, CompanyName, Package, OfferDate)
                    VALUES 
                    (@StudentName, @RollNumber, @Department, @CGPA, @BatchYear, @Email, @Phone, @PlacementStatus, @CompanyName, @Package, @OfferDate);
                    SELECT SCOPE_IDENTITY();";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentName", student.StudentName);
                    cmd.Parameters.AddWithValue("@RollNumber", student.RollNumber);
                    cmd.Parameters.AddWithValue("@Department", student.Department);
                    cmd.Parameters.AddWithValue("@CGPA", student.CGPA);
                    cmd.Parameters.AddWithValue("@BatchYear", student.BatchYear);
                    cmd.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Phone", (object?)student.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PlacementStatus", student.PlacementStatus);
                    cmd.Parameters.AddWithValue("@CompanyName", (object?)student.CompanyName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Package", (object?)student.Package ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@OfferDate", (object?)student.OfferDate ?? DBNull.Value);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public bool UpdateStudent(Student student)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE Students SET 
                    StudentName = @StudentName,
                    RollNumber = @RollNumber,
                    Department = @Department,
                    CGPA = @CGPA,
                    BatchYear = @BatchYear,
                    Email = @Email,
                    Phone = @Phone,
                    PlacementStatus = @PlacementStatus,
                    CompanyName = @CompanyName,
                    Package = @Package,
                    OfferDate = @OfferDate
                    WHERE StudentId = @StudentId";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentId", student.StudentId);
                    cmd.Parameters.AddWithValue("@StudentName", student.StudentName);
                    cmd.Parameters.AddWithValue("@RollNumber", student.RollNumber);
                    cmd.Parameters.AddWithValue("@Department", student.Department);
                    cmd.Parameters.AddWithValue("@CGPA", student.CGPA);
                    cmd.Parameters.AddWithValue("@BatchYear", student.BatchYear);
                    cmd.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Phone", (object?)student.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PlacementStatus", student.PlacementStatus);
                    cmd.Parameters.AddWithValue("@CompanyName", (object?)student.CompanyName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Package", (object?)student.Package ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@OfferDate", (object?)student.OfferDate ?? DBNull.Value);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteStudent(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Students WHERE StudentId = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ==================== COMPANIES CRUD ====================

        public List<Company> GetAllCompanies()
        {
            var list = new List<Company>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Companies ORDER BY PackageOffered DESC";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapCompany(reader));
                        }
                    }
                }
            }
            return list;
        }

        public Company? GetCompanyById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Companies WHERE CompanyId = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapCompany(reader);
                        }
                    }
                }
            }
            return null;
        }

        public int AddCompany(Company company)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Companies 
                    (CompanyName, Industry, MinCGPA, PackageOffered, StudentsHired, DriveDate, Location, Website)
                    VALUES 
                    (@CompanyName, @Industry, @MinCGPA, @PackageOffered, @StudentsHired, @DriveDate, @Location, @Website);
                    SELECT SCOPE_IDENTITY();";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyName", company.CompanyName);
                    cmd.Parameters.AddWithValue("@Industry", company.Industry);
                    cmd.Parameters.AddWithValue("@MinCGPA", company.MinCGPA);
                    cmd.Parameters.AddWithValue("@PackageOffered", company.PackageOffered);
                    cmd.Parameters.AddWithValue("@StudentsHired", company.StudentsHired);
                    cmd.Parameters.AddWithValue("@DriveDate", company.DriveDate);
                    cmd.Parameters.AddWithValue("@Location", (object?)company.Location ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Website", (object?)company.Website ?? DBNull.Value);

                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public bool UpdateCompany(Company company)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE Companies SET 
                    CompanyName = @CompanyName,
                    Industry = @Industry,
                    MinCGPA = @MinCGPA,
                    PackageOffered = @PackageOffered,
                    StudentsHired = @StudentsHired,
                    DriveDate = @DriveDate,
                    Location = @Location,
                    Website = @Website
                    WHERE CompanyId = @CompanyId";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", company.CompanyId);
                    cmd.Parameters.AddWithValue("@CompanyName", company.CompanyName);
                    cmd.Parameters.AddWithValue("@Industry", company.Industry);
                    cmd.Parameters.AddWithValue("@MinCGPA", company.MinCGPA);
                    cmd.Parameters.AddWithValue("@PackageOffered", company.PackageOffered);
                    cmd.Parameters.AddWithValue("@StudentsHired", company.StudentsHired);
                    cmd.Parameters.AddWithValue("@DriveDate", company.DriveDate);
                    cmd.Parameters.AddWithValue("@Location", (object?)company.Location ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Website", (object?)company.Website ?? DBNull.Value);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteCompany(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Companies WHERE CompanyId = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ==================== DATA CONTROLS & DATA ADAPTER SEARCH ====================

        /// <summary>
        /// Demonstrates SqlDataAdapter + DataTable (Disconnected Data Architecture for Data Controls)
        /// </summary>
        public DataTable GetStudentsDataTable(string? search = null)
        {
            var dt = new DataTable("StudentsTable");
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Students WHERE 1=1 ";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query += " AND (StudentName LIKE @Search OR RollNumber LIKE @Search OR Department LIKE @Search OR CompanyName LIKE @Search) ";
                }
                query += " ORDER BY RollNumber ASC";

                using (var adapter = new SqlDataAdapter(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Search", $"%{search}%");
                    }
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        // ==================== DASHBOARD & CRYSTAL REPORTS STATS ====================

        public DashboardStats GetDashboardStats()
        {
            var stats = new DashboardStats();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // 1. Total Students
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Students", conn))
                {
                    stats.TotalStudents = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                }

                // 2. Placed Students
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Students WHERE PlacementStatus = 'Placed'", conn))
                {
                    stats.PlacedStudents = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                }

                // 3. Unplaced Students
                stats.UnplacedStudents = stats.TotalStudents - stats.PlacedStudents;

                // 4. Highest Package
                using (var cmd = new SqlCommand("SELECT ISNULL(MAX(Package), 0) FROM Students WHERE PlacementStatus = 'Placed'", conn))
                {
                    stats.HighestPackage = Convert.ToDecimal(cmd.ExecuteScalar() ?? 0m);
                }

                // 5. Average Package
                using (var cmd = new SqlCommand("SELECT ISNULL(AVG(Package), 0) FROM Students WHERE PlacementStatus = 'Placed'", conn))
                {
                    stats.AveragePackage = Math.Round(Convert.ToDecimal(cmd.ExecuteScalar() ?? 0m), 2);
                }

                // 6. Total Companies
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Companies", conn))
                {
                    stats.TotalCompanies = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                }

                // 7. Department Breakdown using SqlDataAdapter
                var deptDt = new DataTable();
                var deptQuery = @"
                    SELECT 
                        Department,
                        COUNT(*) AS TotalStudents,
                        SUM(CASE WHEN PlacementStatus = 'Placed' THEN 1 ELSE 0 END) AS PlacedCount,
                        ISNULL(AVG(CASE WHEN PlacementStatus = 'Placed' THEN Package ELSE NULL END), 0) AS AvgPackage
                    FROM Students
                    GROUP BY Department
                    ORDER BY Department";

                using (var adapter = new SqlDataAdapter(deptQuery, conn))
                {
                    adapter.Fill(deptDt);
                }

                foreach (DataRow row in deptDt.Rows)
                {
                    stats.DepartmentBreakdown.Add(new DepartmentStat
                    {
                        Department = row["Department"].ToString() ?? "",
                        TotalStudents = Convert.ToInt32(row["TotalStudents"]),
                        PlacedCount = Convert.ToInt32(row["PlacedCount"]),
                        AvgPackage = Math.Round(Convert.ToDecimal(row["AvgPackage"]), 2)
                    });
                }

                // 8. Recent Placements (Top 6)
                using (var cmd = new SqlCommand("SELECT TOP 6 * FROM Students WHERE PlacementStatus = 'Placed' ORDER BY OfferDate DESC, StudentId DESC", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stats.RecentPlacements.Add(MapStudent(reader));
                        }
                    }
                }

                // 9. Top Recruiters (Top 5)
                using (var cmd = new SqlCommand("SELECT TOP 5 * FROM Companies ORDER BY PackageOffered DESC", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stats.TopRecruiters.Add(MapCompany(reader));
                        }
                    }
                }
            }

            return stats;
        }

        public List<string> GetDistinctDepartments()
        {
            var list = new List<string>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = "SELECT DISTINCT Department FROM Students ORDER BY Department";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return list;
        }

        // ==================== MAPPERS ====================

        private static Student MapStudent(SqlDataReader reader)
        {
            return new Student
            {
                StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                RollNumber = reader.GetString(reader.GetOrdinal("RollNumber")),
                Department = reader.GetString(reader.GetOrdinal("Department")),
                CGPA = reader.GetDecimal(reader.GetOrdinal("CGPA")),
                BatchYear = reader.GetString(reader.GetOrdinal("BatchYear")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                PlacementStatus = reader.GetString(reader.GetOrdinal("PlacementStatus")),
                CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                Package = reader.IsDBNull(reader.GetOrdinal("Package")) ? null : reader.GetDecimal(reader.GetOrdinal("Package")),
                OfferDate = reader.IsDBNull(reader.GetOrdinal("OfferDate")) ? null : reader.GetDateTime(reader.GetOrdinal("OfferDate"))
            };
        }

        private static Company MapCompany(SqlDataReader reader)
        {
            return new Company
            {
                CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId")),
                CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                Industry = reader.GetString(reader.GetOrdinal("Industry")),
                MinCGPA = reader.GetDecimal(reader.GetOrdinal("MinCGPA")),
                PackageOffered = reader.GetDecimal(reader.GetOrdinal("PackageOffered")),
                StudentsHired = reader.GetInt32(reader.GetOrdinal("StudentsHired")),
                DriveDate = reader.GetDateTime(reader.GetOrdinal("DriveDate")),
                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                Website = reader.IsDBNull(reader.GetOrdinal("Website")) ? null : reader.GetString(reader.GetOrdinal("Website"))
            };
        }
    }
}
