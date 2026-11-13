-- ============================================================
-- Placement Dashboard System - Database Setup Script
-- ASP.NET MVC + ADO.NET | Data Controls & Crystal Reports
-- ============================================================

-- Step 1: Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PlacementDB')
BEGIN
    CREATE DATABASE PlacementDB;
END
GO

USE PlacementDB;
GO

-- Step 2: Create Students Table
IF OBJECT_ID('Students', 'U') IS NOT NULL DROP TABLE Students;
GO

CREATE TABLE Students (
    StudentId       INT IDENTITY(1,1) PRIMARY KEY,
    StudentName     NVARCHAR(100)   NOT NULL,
    RollNumber      NVARCHAR(20)    NOT NULL UNIQUE,
    Department      NVARCHAR(50)    NOT NULL,
    CGPA            DECIMAL(4, 2)   NOT NULL,
    BatchYear       NVARCHAR(10)    NOT NULL DEFAULT '2024-2025',
    Email           NVARCHAR(100)   NULL,
    Phone           NVARCHAR(15)    NULL,
    PlacementStatus NVARCHAR(20)    NOT NULL DEFAULT 'Unplaced', -- 'Placed', 'Unplaced'
    CompanyName     NVARCHAR(100)   NULL,
    Package         DECIMAL(10, 2)  NULL, -- in LPA
    OfferDate       DATE            NULL
);
GO

-- Step 3: Create Companies Table
IF OBJECT_ID('Companies', 'U') IS NOT NULL DROP TABLE Companies;
GO

CREATE TABLE Companies (
    CompanyId       INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName     NVARCHAR(100)   NOT NULL,
    Industry        NVARCHAR(50)    NOT NULL,
    MinCGPA         DECIMAL(4, 2)   NOT NULL DEFAULT 6.0,
    PackageOffered  DECIMAL(10, 2)  NOT NULL, -- in LPA
    StudentsHired   INT             NOT NULL DEFAULT 0,
    DriveDate       DATE            NOT NULL,
    Location        NVARCHAR(100)   NULL,
    Website         NVARCHAR(150)   NULL
);
GO

-- Step 4: Insert Sample Companies
INSERT INTO Companies (CompanyName, Industry, MinCGPA, PackageOffered, StudentsHired, DriveDate, Location, Website) VALUES
('Google India',            'IT & Software',        8.5, 34.50, 2, '2024-09-15', 'Bangalore',  'https://careers.google.com'),
('Microsoft',               'IT & Software',        8.0, 31.00, 3, '2024-09-22', 'Hyderabad',  'https://careers.microsoft.com'),
('Amazon Development',      'E-Commerce & Cloud',   7.5, 28.00, 2, '2024-10-05', 'Bangalore',  'https://amazon.jobs'),
('Tata Consultancy Services','IT Services',         6.0,  7.00, 6, '2024-10-18', 'Mumbai',     'https://tcs.com/careers'),
('Infosys Technologies',    'IT Services',          6.0,  6.50, 4, '2024-10-25', 'Pune',       'https://infosys.com'),
('Larsen & Toubro (L&T)',   'Core Engineering',     6.5,  8.50, 3, '2024-11-02', 'Vadodara',   'https://larsentoubro.com'),
('Tata Motors',             'Automobile',           6.5,  9.00, 2, '2024-11-10', 'Pune',       'https://tatamotors.com'),
('Siemens India',           'Electrical & Automation', 7.0, 11.00, 2, '2024-11-15', 'Bangalore', 'https://siemens.com'),
('Adani Group',             'Infrastructure',       6.5,  7.50, 2, '2024-11-20', 'Ahmedabad',  'https://adani.com'),
('Cisco Systems',           'Networking & Security',7.5, 18.00, 1, '2024-11-28', 'Bangalore',  'https://cisco.com');
GO

-- Step 5: Insert Sample Students Across Multiple Departments
INSERT INTO Students (StudentName, RollNumber, Department, CGPA, BatchYear, Email, Phone, PlacementStatus, CompanyName, Package, OfferDate) VALUES
-- Computer Engineering
('Rahul Sharma',    '21CE001', 'Computer Engineering',       8.95, '2024-2025', 'rahul.s@college.edu',    '9876543210', 'Placed',   'Google India',             34.50, '2024-09-16'),
('Priya Patel',     '21CE002', 'Computer Engineering',       9.20, '2024-2025', 'priya.p@college.edu',    '9876543211', 'Placed',   'Microsoft',                31.00, '2024-09-23'),
('Amit Verma',      '21CE003', 'Computer Engineering',       7.80, '2024-2025', 'amit.v@college.edu',     '9876543212', 'Placed',   'Tata Consultancy Services',  7.00, '2024-10-19'),
('Ananya Gupta',    '21CE004', 'Computer Engineering',       8.65, '2024-2025', 'ananya.g@college.edu',   '9876543213', 'Placed',   'Amazon Development',       28.00, '2024-10-06'),
('Devendra Joshi',  '21CE005', 'Computer Engineering',       6.80, '2024-2025', 'dev.j@college.edu',      '9876543214', 'Unplaced', NULL,                        NULL,  NULL),

-- Information Technology
('Kavita Nair',     '21IT001', 'Information Technology',    9.10, '2024-2025', 'kavita.n@college.edu',   '9876543215', 'Placed',   'Microsoft',                31.00, '2024-09-23'),
('Rohan Mehta',     '21IT002', 'Information Technology',    8.35, '2024-2025', 'rohan.m@college.edu',    '9876543216', 'Placed',   'Cisco Systems',            18.00, '2024-11-29'),
('Sneha Trivedi',   '21IT003', 'Information Technology',    7.50, '2024-2025', 'sneha.t@college.edu',    '9876543217', 'Placed',   'Infosys Technologies',      6.50, '2024-10-26'),
('Vikram Singh',    '21IT004', 'Information Technology',    6.45, '2024-2025', 'vikram.s@college.edu',   '9876543218', 'Unplaced', NULL,                        NULL,  NULL),

-- Mechanical Engineering
('Arjun Dave',      '21ME001', 'Mechanical Engineering',     8.15, '2024-2025', 'arjun.d@college.edu',    '9876543219', 'Placed',   'Tata Motors',               9.00, '2024-11-11'),
('Hardik Shah',     '21ME002', 'Mechanical Engineering',     7.90, '2024-2025', 'hardik.s@college.edu',   '9876543220', 'Placed',   'Larsen & Toubro (L&T)',     8.50, '2024-11-03'),
('Manish Kumar',    '21ME003', 'Mechanical Engineering',     6.90, '2024-2025', 'manish.k@college.edu',   '9876543221', 'Unplaced', NULL,                        NULL,  NULL),
('Jayesh Parmar',   '21ME004', 'Mechanical Engineering',     6.30, '2024-2025', 'jayesh.p@college.edu',   '9876543222', 'Unplaced', NULL,                        NULL,  NULL),

-- Electrical Engineering
('Divya Rao',       '21EE001', 'Electrical Engineering',     8.75, '2024-2025', 'divya.r@college.edu',    '9876543223', 'Placed',   'Siemens India',            11.00, '2024-11-16'),
('Karan Makwana',   '21EE002', 'Electrical Engineering',     7.60, '2024-2025', 'karan.m@college.edu',    '9876543224', 'Placed',   'Tata Consultancy Services',  7.00, '2024-10-19'),
('Sonal Solanki',   '21EE003', 'Electrical Engineering',     6.70, '2024-2025', 'sonal.s@college.edu',    '9876543225', 'Unplaced', NULL,                        NULL,  NULL),

-- Civil Engineering
('Nilesh Yadav',    '21CL001', 'Civil Engineering',          8.20, '2024-2025', 'nilesh.y@college.edu',   '9876543226', 'Placed',   'Adani Group',               7.50, '2024-11-21'),
('Pooja Chauhan',   '21CL002', 'Civil Engineering',          7.40, '2024-2025', 'pooja.c@college.edu',    '9876543227', 'Placed',   'Larsen & Toubro (L&T)',     8.50, '2024-11-03'),
('Vikas Rathod',    '21CL003', 'Civil Engineering',          6.10, '2024-2025', 'vikas.r@college.edu',    '9876543228', 'Unplaced', NULL,                        NULL,  NULL);
GO

-- Verify Data
SELECT * FROM Students;
SELECT * FROM Companies;
GO
