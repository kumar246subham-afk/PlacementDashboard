# 🎓 Campus Placement Dashboard using Data Controls & Crystal Reports

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0_MVC-blue?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-purple?style=for-the-badge&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2022-red?style=for-the-badge&logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![ADO.NET](https://img.shields.io/badge/ADO.NET-Connected_%26_Disconnected-green?style=for-the-badge)](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-blueviolet?style=for-the-badge&logo=bootstrap)](https://getbootstrap.com/)

An enterprise-grade **College Campus Placement Management & Analytical Dashboard** built for the **ASP.NET Technology** curriculum, demonstrating **Data Controls (GridView, DetailsView, FormView, Repeater, DataList)** and **Crystal Reports** architecture with raw **ADO.NET** data persistence.

---

## 📌 Project Highlights & Syllabus Coverage

### 1. Data Controls Equivalents & Design Patterns
In modern web applications, the classic ASP.NET Web Forms data controls are cleanly mapped and demonstrated:

| Classic ASP.NET Data Control | Modern Implementation in this Project | Location |
| :--- | :--- | :--- |
| **GridView** | Searchable, filterable, responsive data grid with action columns | [`Views/Student/Index.cshtml`](Views/Student/Index.cshtml) |
| **DetailsView** | Single record inspection with read-only master profile view | [`Views/Student/Details.cshtml`](Views/Student/Details.cshtml) |
| **FormView** | Data-bound input/edit templates with validation rules | [`Views/Student/Create.cshtml`](Views/Student/Create.cshtml) & `Edit.cshtml` |
| **Repeater** | Lightweight, template-driven branch placement progress list | [`Views/Dashboard/Index.cshtml`](Views/Dashboard/Index.cshtml) |
| **DataList** | Multi-column corporate recruiters card layout | [`Views/Dashboard/Index.cshtml`](Views/Dashboard/Index.cshtml) |

---

### 2. Crystal Reports Layout & Reporting Engine
The project features an authentic **Crystal Reports** reporting module at [`Views/Report/PlacementReport.cshtml`](Views/Report/PlacementReport.cshtml):
- **Report Header**: Institutional banner, AICTE approval details, timestamp, report scope.
- **KPI Summary Strip**: Total offers, highest package, institutional average CTC.
- **Group Header**: Grouped by academic **Department** (e.g., Computer Engineering, Information Technology, Mechanical, Electrical, Civil).
- **Details Section**: Tabular rows showing Roll Number, Student Name, CGPA, Recruiting Company, Offered Package (LPA), and Offer Date.
- **Group Footer**: Branch sub-totals calculating total placements, department average salary, and branch highest package.
- **Report Footer**: Grand totals across all branches.
- **Audit & Signatures Block**: Official verification signatures for Placement Coordinator, Head of T&P Cell (TPO), and Principal.
- **Print / PDF Engine**: Dedicated CSS `@media print` rules removing UI navigation and formatting the report strictly to A4 page standards.

---

### 3. ADO.NET Architecture
The data access layer [`PlacementRepository.cs`](Data/PlacementRepository.cs) demonstrates full ADO.NET classes without Entity Framework:

```csharp
// 1. SqlConnection: Connection management
using SqlConnection conn = new SqlConnection(_connectionString);

// 2. SqlCommand with Parameters: Prevents SQL Injection
using SqlCommand cmd = new SqlCommand(query, conn);
cmd.Parameters.AddWithValue("@StudentName", student.StudentName);

// 3. SqlDataReader: Fast, forward-only connected reads
using SqlDataReader reader = cmd.ExecuteReader();
while (reader.Read()) { ... }

// 4. SqlDataAdapter & DataTable: Disconnected data for Search & GridView binding
using SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
DataTable dt = new DataTable();
adapter.Fill(dt);

// 5. ExecuteScalar: Retrieves aggregate counts & SCOPE_IDENTITY()
int newId = Convert.ToInt32(cmd.ExecuteScalar());

// 6. ExecuteNonQuery: Performs UPDATE and DELETE operations
int rowsAffected = cmd.ExecuteNonQuery();
```

---

## 🚀 Key Features

- **Executive Analytics Dashboard**:
  - 6 KPI metric cards: Total Students, Placed, Unplaced, Placement Rate %, Highest CTC, Average Package.
  - Interactive status ratio doughnut chart powered by Chart.js.
  - Department-wise placement performance with progress bars.
  - Recent campus selections grid and top recruiters list.
- **Student Placement Management (CRUD)**:
  - Register new students with academic CGPA, batch, and contact details.
  - Record campus job offers with recruiter name, package (LPA), and offer date.
  - Filter students by branch, placement status, or search term via ADO.NET `SqlDataAdapter`.
- **Corporate Recruiter Management**:
  - Add visiting companies, eligible minimum CGPA, package offered, drive date, and hired counts.
- **Crystal Reports Hub**:
  - Department-wise Grouped Placement Report with sub-totals and print trigger.
  - Corporate Recruiter Drive & Hiring Audit Report.

---

## 🗄️ Database Architecture

The system uses **Microsoft SQL Server** with parameterized schema in [`Database/PlacementDB_Setup.sql`](Database/PlacementDB_Setup.sql):

```sql
CREATE TABLE Students (
    StudentId       INT IDENTITY(1,1) PRIMARY KEY,
    StudentName     NVARCHAR(100)   NOT NULL,
    RollNumber      NVARCHAR(20)    NOT NULL UNIQUE,
    Department      NVARCHAR(50)    NOT NULL,
    CGPA            DECIMAL(4, 2)   NOT NULL,
    BatchYear       NVARCHAR(10)    NOT NULL DEFAULT '2024-2025',
    Email           NVARCHAR(100)   NULL,
    Phone           NVARCHAR(15)    NULL,
    PlacementStatus NVARCHAR(20)    NOT NULL DEFAULT 'Unplaced',
    CompanyName     NVARCHAR(100)   NULL,
    Package         DECIMAL(10, 2)  NULL,
    OfferDate       DATE            NULL
);

CREATE TABLE Companies (
    CompanyId       INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName     NVARCHAR(100)   NOT NULL,
    Industry        NVARCHAR(50)    NOT NULL,
    MinCGPA         DECIMAL(4, 2)   NOT NULL DEFAULT 6.0,
    PackageOffered  DECIMAL(10, 2)  NOT NULL,
    StudentsHired   INT             NOT NULL DEFAULT 0,
    DriveDate       DATE            NOT NULL,
    Location        NVARCHAR(100)   NULL,
    Website         NVARCHAR(150)   NULL
);
```

---

## 📂 Project Structure

```
PlacementDashboard/
├── Controllers/
│   ├── DashboardController.cs     # Main executive dashboard
│   ├── StudentController.cs       # Full CRUD operations (GridView, DetailsView, FormView)
│   ├── CompanyController.cs       # Recruiter & drive management
│   └── ReportController.cs        # Crystal Reports generation & grouping
├── Data/
│   └── PlacementRepository.cs     # ADO.NET Data Access Layer (DAL)
├── Models/
│   ├── Student.cs                 # Student model with Data Annotations
│   ├── Company.cs                 # Company recruiter model
│   └── DashboardStats.cs          # Aggregation & department metrics model
├── Views/
│   ├── Dashboard/
│   │   └── Index.cshtml           # KPI cards, charts, Repeater & DataList views
│   ├── Student/
│   │   ├── Index.cshtml           # GridView style list with search & filter
│   │   ├── Create.cshtml          # FormView insert template
│   │   ├── Edit.cshtml            # FormView edit template
│   │   ├── Details.cshtml         # DetailsView single record template
│   │   └── Delete.cshtml          # Delete confirmation screen
│   ├── Company/
│   │   ├── Index.cshtml           # Recruiter list
│   │   ├── Create.cshtml          # Add company
│   │   ├── Edit.cshtml            # Update company
│   │   └── Delete.cshtml          # Remove company
│   ├── Report/
│   │   ├── Index.cshtml           # Crystal Reports hub
│   │   ├── PlacementReport.cshtml # Grouped Crystal Report layout with print
│   │   └── CompanyWiseReport.cshtml # Corporate visits report
│   └── Shared/
│       └── _Layout.cshtml         # Responsive dark sidebar + print stylesheet
├── Database/
│   └── PlacementDB_Setup.sql      # Database creation & sample data script
├── appsettings.json               # Connection string configuration
└── Program.cs                     # Startup and DI registration
```

---

## ⚙️ How to Setup & Run

### 1. Database Setup
1. Open **SQL Server Management Studio (SSMS)** or Azure Data Studio.
2. Open and execute the script:
   ```
   Database/PlacementDB_Setup.sql
   ```
3. This creates `PlacementDB` and seeds it with 20+ realistic student records and top recruiter companies.

### 2. Configure Connection String
Check `appsettings.json` to verify the connection string matches your local SQL Server instance:
```json
"ConnectionStrings": {
  "PlacementDB": "Server=(localdb)\\mssqllocaldb;Database=PlacementDB;Trusted_Connection=True;"
}
```
*(If using SQL Server Express, set `Server=.\\SQLEXPRESS;`)*

### 3. Run the Web Application
```bash
cd PlacementDashboard
dotnet restore
dotnet run
```
Open your browser and navigate to:
```
http://localhost:5000
```

---

## 👨‍💻 Developer Information

- **Developer**: Subham Kumar Keshari
- **Degree**: B.Tech in Computer Engineering
- **GitHub**: [@kumar246subham-afk](https://github.com/kumar246subham-afk)
- **Email**: kumar246subham@gmail.com
- **Portfolio**: [my-portfolio-yyh1.vercel.app](https://my-portfolio-yyh1.vercel.app/)
- **Academic Subject**: ASP.NET Technology — *Placement Dashboard using Data Controls & Crystal Reports*
