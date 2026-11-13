using System.Data;
using System.Data.SqlClient;
using PlacementDashboard.Models;

namespace PlacementDashboard.Data
{
    /// <summary>
    /// Data Access Layer using ADO.NET for Placement Dashboard and Crystal Reports.
    /// Includes ADO.NET connected & disconnected modes with in-memory fallback for instant browser testing.
    /// </summary>
    public class PlacementRepository
    {
        private readonly string _connectionString;
        private static readonly List<Student> _inMemoryStudents = new();
        private static readonly List<Company> _inMemoryCompanies = new();
        private static bool _seeded = false;

        public PlacementRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PlacementDB")
                ?? "Server=(localdb)\\mssqllocaldb;Database=PlacementDB;Trusted_Connection=True;";
            EnsureSeedData();
        }

        private static void EnsureSeedData()
        {
            if (_seeded) return;
            lock (_inMemoryStudents)
            {
                if (_seeded) return;

                _inMemoryCompanies.AddRange(new[]
                {
                    new Company { CompanyId = 1, CompanyName = "Google India", Industry = "IT & Software", MinCGPA = 8.5m, PackageOffered = 34.50m, StudentsHired = 2, DriveDate = new DateTime(2024, 9, 15), Location = "Bangalore", Website = "https://careers.google.com" },
                    new Company { CompanyId = 2, CompanyName = "Microsoft", Industry = "IT & Software", MinCGPA = 8.0m, PackageOffered = 31.00m, StudentsHired = 3, DriveDate = new DateTime(2024, 9, 22), Location = "Hyderabad", Website = "https://careers.microsoft.com" },
                    new Company { CompanyId = 3, CompanyName = "Amazon Development", Industry = "E-Commerce & Cloud", MinCGPA = 7.5m, PackageOffered = 28.00m, StudentsHired = 2, DriveDate = new DateTime(2024, 10, 5), Location = "Bangalore", Website = "https://amazon.jobs" },
                    new Company { CompanyId = 4, CompanyName = "Tata Consultancy Services", Industry = "IT Services", MinCGPA = 6.0m, PackageOffered = 7.00m, StudentsHired = 6, DriveDate = new DateTime(2024, 10, 18), Location = "Mumbai", Website = "https://tcs.com" },
                    new Company { CompanyId = 5, CompanyName = "Infosys Technologies", Industry = "IT Services", MinCGPA = 6.0m, PackageOffered = 6.50m, StudentsHired = 4, DriveDate = new DateTime(2024, 10, 25), Location = "Pune", Website = "https://infosys.com" },
                    new Company { CompanyId = 6, CompanyName = "Larsen & Toubro (L&T)", Industry = "Core Engineering", MinCGPA = 6.5m, PackageOffered = 8.50m, StudentsHired = 3, DriveDate = new DateTime(2024, 11, 2), Location = "Vadodara", Website = "https://larsentoubro.com" },
                    new Company { CompanyId = 7, CompanyName = "Tata Motors", Industry = "Automobile", MinCGPA = 6.5m, PackageOffered = 9.00m, StudentsHired = 2, DriveDate = new DateTime(2024, 11, 10), Location = "Pune", Website = "https://tatamotors.com" },
                    new Company { CompanyId = 8, CompanyName = "Siemens India", Industry = "Electrical & Automation", MinCGPA = 7.0m, PackageOffered = 11.00m, StudentsHired = 2, DriveDate = new DateTime(2024, 11, 15), Location = "Bangalore", Website = "https://siemens.com" },
                    new Company { CompanyId = 9, CompanyName = "Adani Group", Industry = "Infrastructure", MinCGPA = 6.5m, PackageOffered = 7.50m, StudentsHired = 2, DriveDate = new DateTime(2024, 11, 20), Location = "Ahmedabad", Website = "https://adani.com" },
                    new Company { CompanyId = 10, CompanyName = "Cisco Systems", Industry = "Networking & Security", MinCGPA = 7.5m, PackageOffered = 18.00m, StudentsHired = 1, DriveDate = new DateTime(2024, 11, 28), Location = "Bangalore", Website = "https://cisco.com" }
                });

                _inMemoryStudents.AddRange(new[]
                {
                    new Student { StudentId = 1, StudentName = "Rahul Sharma", RollNumber = "21CE001", Department = "Computer Engineering", CGPA = 8.95m, BatchYear = "2024-2025", Email = "rahul.s@college.edu", Phone = "9876543210", PlacementStatus = "Placed", CompanyName = "Google India", Package = 34.50m, OfferDate = new DateTime(2024, 9, 16) },
                    new Student { StudentId = 2, StudentName = "Priya Patel", RollNumber = "21CE002", Department = "Computer Engineering", CGPA = 9.20m, BatchYear = "2024-2025", Email = "priya.p@college.edu", Phone = "9876543211", PlacementStatus = "Placed", CompanyName = "Microsoft", Package = 31.00m, OfferDate = new DateTime(2024, 9, 23) },
                    new Student { StudentId = 3, StudentName = "Amit Verma", RollNumber = "21CE003", Department = "Computer Engineering", CGPA = 7.80m, BatchYear = "2024-2025", Email = "amit.v@college.edu", Phone = "9876543212", PlacementStatus = "Placed", CompanyName = "Tata Consultancy Services", Package = 7.00m, OfferDate = new DateTime(2024, 10, 19) },
                    new Student { StudentId = 4, StudentName = "Ananya Gupta", RollNumber = "21CE004", Department = "Computer Engineering", CGPA = 8.65m, BatchYear = "2024-2025", Email = "ananya.g@college.edu", Phone = "9876543213", PlacementStatus = "Placed", CompanyName = "Amazon Development", Package = 28.00m, OfferDate = new DateTime(2024, 10, 6) },
                    new Student { StudentId = 5, StudentName = "Devendra Joshi", RollNumber = "21CE005", Department = "Computer Engineering", CGPA = 6.80m, BatchYear = "2024-2025", Email = "dev.j@college.edu", Phone = "9876543214", PlacementStatus = "Unplaced" },
                    new Student { StudentId = 6, StudentName = "Kavita Nair", RollNumber = "21IT001", Department = "Information Technology", CGPA = 9.10m, BatchYear = "2024-2025", Email = "kavita.n@college.edu", Phone = "9876543215", PlacementStatus = "Placed", CompanyName = "Microsoft", Package = 31.00m, OfferDate = new DateTime(2024, 9, 23) },
                    new Student { StudentId = 7, StudentName = "Rohan Mehta", RollNumber = "21IT002", Department = "Information Technology", CGPA = 8.35m, BatchYear = "2024-2025", Email = "rohan.m@college.edu", Phone = "9876543216", PlacementStatus = "Placed", CompanyName = "Cisco Systems", Package = 18.00m, OfferDate = new DateTime(2024, 11, 29) },
                    new Student { StudentId = 8, StudentName = "Sneha Trivedi", RollNumber = "21IT003", Department = "Information Technology", CGPA = 7.50m, BatchYear = "2024-2025", Email = "sneha.t@college.edu", Phone = "9876543217", PlacementStatus = "Placed", CompanyName = "Infosys Technologies", Package = 6.50m, OfferDate = new DateTime(2024, 10, 26) },
                    new Student { StudentId = 9, StudentName = "Vikram Singh", RollNumber = "21IT004", Department = "Information Technology", CGPA = 6.45m, BatchYear = "2024-2025", Email = "vikram.s@college.edu", Phone = "9876543218", PlacementStatus = "Unplaced" },
                    new Student { StudentId = 10, StudentName = "Arjun Dave", RollNumber = "21ME001", Department = "Mechanical Engineering", CGPA = 8.15m, BatchYear = "2024-2025", Email = "arjun.d@college.edu", Phone = "9876543219", PlacementStatus = "Placed", CompanyName = "Tata Motors", Package = 9.00m, OfferDate = new DateTime(2024, 11, 11) },
                    new Student { StudentId = 11, StudentName = "Hardik Shah", RollNumber = "21ME002", Department = "Mechanical Engineering", CGPA = 7.90m, BatchYear = "2024-2025", Email = "hardik.s@college.edu", Phone = "9876543220", PlacementStatus = "Placed", CompanyName = "Larsen & Toubro (L&T)", Package = 8.50m, OfferDate = new DateTime(2024, 11, 3) },
                    new Student { StudentId = 12, StudentName = "Manish Kumar", RollNumber = "21ME003", Department = "Mechanical Engineering", CGPA = 6.90m, BatchYear = "2024-2025", Email = "manish.k@college.edu", Phone = "9876543221", PlacementStatus = "Unplaced" },
                    new Student { StudentId = 13, StudentName = "Divya Rao", RollNumber = "21EE001", Department = "Electrical Engineering", CGPA = 8.75m, BatchYear = "2024-2025", Email = "divya.r@college.edu", Phone = "9876543223", PlacementStatus = "Placed", CompanyName = "Siemens India", Package = 11.00m, OfferDate = new DateTime(2024, 11, 16) },
                    new Student { StudentId = 14, StudentName = "Karan Makwana", RollNumber = "21EE002", Department = "Electrical Engineering", CGPA = 7.60m, BatchYear = "2024-2025", Email = "karan.m@college.edu", Phone = "9876543224", PlacementStatus = "Placed", CompanyName = "Tata Consultancy Services", Package = 7.00m, OfferDate = new DateTime(2024, 10, 19) },
                    new Student { StudentId = 15, StudentName = "Sonal Solanki", RollNumber = "21EE003", Department = "Electrical Engineering", CGPA = 6.70m, BatchYear = "2024-2025", Email = "sonal.s@college.edu", Phone = "9876543225", PlacementStatus = "Unplaced" },
                    new Student { StudentId = 16, StudentName = "Nilesh Yadav", RollNumber = "21CL001", Department = "Civil Engineering", CGPA = 8.20m, BatchYear = "2024-2025", Email = "nilesh.y@college.edu", Phone = "9876543226", PlacementStatus = "Placed", CompanyName = "Adani Group", Package = 7.50m, OfferDate = new DateTime(2024, 11, 21) },
                    new Student { StudentId = 17, StudentName = "Pooja Chauhan", RollNumber = "21CL002", Department = "Civil Engineering", CGPA = 7.40m, BatchYear = "2024-2025", Email = "pooja.c@college.edu", Phone = "9876543227", PlacementStatus = "Placed", CompanyName = "Larsen & Toubro (L&T)", Package = 8.50m, OfferDate = new DateTime(2024, 11, 3) },
                    new Student { StudentId = 18, StudentName = "Vikas Rathod", RollNumber = "21CL003", Department = "Civil Engineering", CGPA = 6.10m, BatchYear = "2024-2025", Email = "vikas.r@college.edu", Phone = "9876543228", PlacementStatus = "Unplaced" }
                });

                _seeded = true;
            }
        }

        // ==================== STUDENTS CRUD ====================

        public List<Student> GetAllStudents(string? department = null, string? status = null, string? search = null)
        {
            try
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
            catch
            {
                // In-memory fallback
                var query = _inMemoryStudents.AsEnumerable();
                if (!string.IsNullOrWhiteSpace(department)) query = query.Where(s => s.Department == department);
                if (!string.IsNullOrWhiteSpace(status)) query = query.Where(s => s.PlacementStatus == status);
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var sLower = search.ToLower();
                    query = query.Where(s => s.StudentName.ToLower().Contains(sLower) || s.RollNumber.ToLower().Contains(sLower) || (s.CompanyName != null && s.CompanyName.ToLower().Contains(sLower)));
                }
                return query.OrderBy(s => s.RollNumber).ToList();
            }
        }

        public Student? GetStudentById(int id)
        {
            try
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
                            if (reader.Read()) return MapStudent(reader);
                        }
                    }
                }
            }
            catch { }

            return _inMemoryStudents.FirstOrDefault(s => s.StudentId == id);
        }

        public int AddStudent(Student student)
        {
            try
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
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch
            {
                student.StudentId = _inMemoryStudents.Count > 0 ? _inMemoryStudents.Max(s => s.StudentId) + 1 : 1;
                _inMemoryStudents.Add(student);
                return student.StudentId;
            }
        }

        public bool UpdateStudent(Student student)
        {
            try
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
            catch
            {
                var existing = _inMemoryStudents.FirstOrDefault(s => s.StudentId == student.StudentId);
                if (existing != null)
                {
                    existing.StudentName = student.StudentName;
                    existing.RollNumber = student.RollNumber;
                    existing.Department = student.Department;
                    existing.CGPA = student.CGPA;
                    existing.BatchYear = student.BatchYear;
                    existing.Email = student.Email;
                    existing.Phone = student.Phone;
                    existing.PlacementStatus = student.PlacementStatus;
                    existing.CompanyName = student.CompanyName;
                    existing.Package = student.Package;
                    existing.OfferDate = student.OfferDate;
                    return true;
                }
                return false;
            }
        }

        public bool DeleteStudent(int id)
        {
            try
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
            catch
            {
                var item = _inMemoryStudents.FirstOrDefault(s => s.StudentId == id);
                if (item != null)
                {
                    _inMemoryStudents.Remove(item);
                    return true;
                }
                return false;
            }
        }

        // ==================== COMPANIES CRUD ====================

        public List<Company> GetAllCompanies()
        {
            try
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
                            while (reader.Read()) list.Add(MapCompany(reader));
                        }
                    }
                }
                return list;
            }
            catch
            {
                return _inMemoryCompanies.OrderByDescending(c => c.PackageOffered).ToList();
            }
        }

        public Company? GetCompanyById(int id)
        {
            try
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
                            if (reader.Read()) return MapCompany(reader);
                        }
                    }
                }
            }
            catch { }

            return _inMemoryCompanies.FirstOrDefault(c => c.CompanyId == id);
        }

        public int AddCompany(Company company)
        {
            try
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
            catch
            {
                company.CompanyId = _inMemoryCompanies.Count > 0 ? _inMemoryCompanies.Max(c => c.CompanyId) + 1 : 1;
                _inMemoryCompanies.Add(company);
                return company.CompanyId;
            }
        }

        public bool UpdateCompany(Company company)
        {
            try
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
            catch
            {
                var existing = _inMemoryCompanies.FirstOrDefault(c => c.CompanyId == company.CompanyId);
                if (existing != null)
                {
                    existing.CompanyName = company.CompanyName;
                    existing.Industry = company.Industry;
                    existing.MinCGPA = company.MinCGPA;
                    existing.PackageOffered = company.PackageOffered;
                    existing.StudentsHired = company.StudentsHired;
                    existing.DriveDate = company.DriveDate;
                    existing.Location = company.Location;
                    existing.Website = company.Website;
                    return true;
                }
                return false;
            }
        }

        public bool DeleteCompany(int id)
        {
            try
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
            catch
            {
                var item = _inMemoryCompanies.FirstOrDefault(c => c.CompanyId == id);
                if (item != null)
                {
                    _inMemoryCompanies.Remove(item);
                    return true;
                }
                return false;
            }
        }

        // ==================== DASHBOARD & STATS ====================

        public DashboardStats GetDashboardStats()
        {
            var students = GetAllStudents();
            var companies = GetAllCompanies();

            var stats = new DashboardStats
            {
                TotalStudents = students.Count,
                PlacedStudents = students.Count(s => s.PlacementStatus == "Placed"),
                TotalCompanies = companies.Count
            };
            stats.UnplacedStudents = stats.TotalStudents - stats.PlacedStudents;

            var placedList = students.Where(s => s.PlacementStatus == "Placed").ToList();
            if (placedList.Any())
            {
                stats.HighestPackage = placedList.Max(s => s.Package ?? 0);
                stats.AveragePackage = Math.Round(placedList.Average(s => s.Package ?? 0), 2);
            }

            // Department Breakdown
            stats.DepartmentBreakdown = students
                .GroupBy(s => s.Department)
                .Select(g => new DepartmentStat
                {
                    Department = g.Key,
                    TotalStudents = g.Count(),
                    PlacedCount = g.Count(s => s.PlacementStatus == "Placed"),
                    AvgPackage = g.Where(s => s.PlacementStatus == "Placed").Any() ? Math.Round(g.Where(s => s.PlacementStatus == "Placed").Average(s => s.Package ?? 0), 2) : 0
                })
                .OrderBy(d => d.Department)
                .ToList();

            stats.RecentPlacements = placedList.OrderByDescending(s => s.OfferDate ?? DateTime.MinValue).Take(6).ToList();
            stats.TopRecruiters = companies.OrderByDescending(c => c.PackageOffered).Take(5).ToList();

            return stats;
        }

        public List<string> GetDistinctDepartments()
        {
            return GetAllStudents().Select(s => s.Department).Distinct().OrderBy(d => d).ToList();
        }

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
