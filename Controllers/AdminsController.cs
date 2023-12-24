using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text;
using ExcelDataReader;
using System.Diagnostics;
using LSMS.Services;
using System.Security.Claims;

namespace LSMS.Controllers
{
    [CustomAuthorize("Admins")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AdminsController : Controller
    {
        private readonly IAuthenticationService authService;
        private readonly ApplicationDbContext dbContext;
        private readonly IScheduleGeneratorService scheduleGeneratorService;

        public AdminsController(IAuthenticationService authService, ApplicationDbContext dbContext, IScheduleGeneratorService scheduleGeneratorService)
        {
            this.authService = authService;
            this.dbContext = dbContext;
            this.scheduleGeneratorService = scheduleGeneratorService;
        }


        [CustomAuthorize("Admins")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Profile()
        {
            ClaimsPrincipal user = HttpContext.User;
            string username = user.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the full professor details from the database using dbContext
            var loggedIn = dbContext.Admins.FirstOrDefault(p => p.userName == username);
            if (loggedIn != null)
            {
                // Pass the professor model to the view
                return View(loggedIn);
            }
            ViewBag.ErrorMessage = "Invalid username or password";
            return RedirectToAction("Logout", "Home");
        }

        public IActionResult UploadExcelStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelStudent(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Create the complete path \\wwwroot\\Uploads\\filename
                var filePath = Path.Combine(uploadsFolder, file.FileName);

                //Copies the content of the uploaded file to the specified file path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var students = new List<Student>();
                        var users = new List<User>();
                        do
                        {
                            bool isHeaderSkipped = false;

                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }
                                // Hashing Password
                                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(reader.GetValue(4).ToString(), salt);

                                var student = new Student
                                {
                                    name = reader.GetValue(0).ToString(),
                                    SSN = reader.GetValue(1).ToString(),
                                    phoneNumber = reader.GetValue(2).ToString(),
                                    academicEmail = reader.GetValue(3).ToString(),
                                    departmentId = (reader.GetValue(5).ToString()),
                                    // Add other properties as needed
                                };
                                var user = new User
                                {
                                    userName = reader.GetValue(1).ToString(),
                                    Salt = Encoding.UTF8.GetBytes(salt),
                                    PasswordHash = Encoding.UTF8.GetBytes(hashedPassword),
                                    role = "Students",
                                };
                                if (dbContext.Students.Contains(student) == false)
                                    students.Add(student);
                                if (dbContext.Users.Contains(user) == false)
                                    users.Add(user);
                            }
                        } while (reader.NextResult());
                        dbContext.Students.AddRange(students);
                        dbContext.Users.AddRange(users);
                        await dbContext.SaveChangesAsync();

                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            return View();
        }

        public IActionResult UploadExcelProfessor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadExcelProfessor(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Create the complete path \\wwwroot\\Uploads\\filename
                var filePath = Path.Combine(uploadsFolder, file.FileName);

                //Copies the content of the uploaded file to the specified file path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var professors = new List<Professor>();
                        var users = new List<User>();
                        do
                        {
                            bool isHeaderSkipped = false;

                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }
                                // Hashing Password
                                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(reader.GetValue(3).ToString(), salt);

                                var professor = new Professor
                                {
                                    name = reader.GetValue(0).ToString(),
                                    SSN = reader.GetValue(1).ToString(),
                                    phoneNumber = reader.GetValue(2).ToString(),
                                    departmentId = (reader.GetValue(4).ToString()),
                                    // Add other properties as needed
                                };
                                var user = new User
                                {
                                    userName = reader.GetValue(1).ToString(),
                                    Salt = Encoding.UTF8.GetBytes(salt),
                                    PasswordHash = Encoding.UTF8.GetBytes(hashedPassword),
                                    role = "Professors",
                                };
                                if (dbContext.Professors.Contains(professor) == false)
                                    professors.Add(professor);
                                if (dbContext.Users.Contains(user) == false)
                                    users.Add(user);
                            }
                        } while (reader.NextResult());
                        dbContext.Professors.AddRange(professors);
                        dbContext.Users.AddRange(users);
                        await dbContext.SaveChangesAsync();

                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            return View();
        }


        public IActionResult UploadExcelCourse()
        {
            var departments = dbContext.Departments.ToList();
            return View(departments);
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelCourse(IFormFile file, string departmentId)
        {
            var departments = dbContext.Departments.ToList();
            if (departmentId == null)
            {
                ViewBag.Message = "emptyDepartment";
                return View(departments);
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Create the complete path \\wwwroot\\Uploads\\filename
                var filePath = Path.Combine(uploadsFolder, file.FileName);

                //Copies the content of the uploaded file to the specified file path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var courses = new List<Course>();
                        do
                        {
                            bool isHeaderSkipped = false;

                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }
                                var course = new Course
                                {
                                    id = reader.GetValue(0).ToString(),
                                    name = reader.GetValue(1).ToString(),
                                    hours = Convert.ToInt32(reader.GetValue(2)),
                                    departmentId = departmentId,
                                    // Add other properties as needed
                                };
                                if (dbContext.Courses.Contains(course) == false)
                                    courses.Add(course);
                            }
                        } while (reader.NextResult());
                        dbContext.Courses.AddRange(courses);
                        await dbContext.SaveChangesAsync();

                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            return View(departments);
        }



        public ActionResult ProfessorTeachCourse()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProfessorTeachCourse(string courseId)
        {
            var course = dbContext.Lectures.Where(e => e.courseId == courseId).Select(e => e.professor).ToList();

            if (course.Count() != 0)
            {
                return View(course.ToList());
            }
            ViewBag.ErrorMessage = "there is no course";
            return View(course);
        }
        public ActionResult Lectureview()
        {
            var lecture = dbContext.Lectures.Include(s => s.students).ToList();
            return View(lecture);
        }
        public ActionResult StudentsEnrolled()
        {
            var student = dbContext.Students.Include(s => s.department).ToList();
            if (student.Count() != 0)
            {
                return View(student);
            }
            ViewBag.ErrorMessage = "there is no students";
            return View(student);
        }

        public IActionResult GenerateSchedule()
        {
            // Call your schedule generation logic
            var lectures = dbContext.Lectures.Include(l => l.students).Include(l => l.professor).ToList();
            var halls = dbContext.Halls.ToList();
            if (halls.Count() * 25 < lectures.Count())
            {
                ViewBag.ErrorMessage = "We can't generate schedule with this small number of halls. " +
                    "please back and add new hall";
                return View();
            }
            else if (halls.Count() >= dbContext.Departments.Count())
            {
                int worst = 0;
                var departments = dbContext.Departments.Include(l => l.courses).ToList();
                foreach (var department in departments)
                {
                    var worstlecture = dbContext.Lectures.Where(l => l.course.departmentId == department.id).Count();
                    var worstintersection = dbContext.Intersections.Where(l => l.departmentId == department.id).Count();

                    if (worstlecture + worstintersection > worst)
                        worst = worstlecture + worstintersection;
                }
                if (worst > 25)
                {
                    ViewBag.ErrorMessage = "We can't generate schedule because you exceeded the intersection limit";
                    return View();
                }
                else scheduleGeneratorService.GenerateScheduleBacktrack(lectures, halls);
            }
            else
            {
                int totalIntersections = dbContext.Intersections.Count(), maxCourses = 0;
                foreach (var dep in dbContext.Departments.Include(l => l.courses).ToList())
                {
                    if (dep.courses.Count() > maxCourses)
                    {
                        maxCourses = dep.courses.Count();
                    }
                }
                if (totalIntersections + maxCourses > 25)
                {

                }
                else scheduleGeneratorService.GenerateScheduleBacktrack(lectures, halls);
            }
            return RedirectToAction("schedule");
        }
        public IActionResult schedule()
        {
            var lectures = new List<Lecture>();
            var halls = dbContext.Halls.ToList();
            ViewBag.Halls = halls;
            return View(lectures);
        }

        [HttpPost]
        public IActionResult schedule(string selectedHall)
        {
            var halls = dbContext.Halls.ToList();
            ViewBag.Halls = halls;
            List<Lecture> lectures = dbContext.Lectures.ToList(); // Implement GetLectures() to retrieve your lectures from the database or another source
            if (!string.IsNullOrEmpty(selectedHall))
            {
                // Filter lectures based on the selected hall
                lectures = lectures.Where(l => l.hallId.ToString() == selectedHall).ToList();
            }
            return View(lectures);
        }

        public IActionResult CreateDepartment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(string id, string name)
        {
            /*
            if (id == null)
            {
                //ViewBag.Message = "emptyId";
                return View();
            }
            else if (name == null)
            {
                //ViewBag.Message = "emptyName";
                return View();
            }
            */
            Department department = new Department();
            department.id = id;
            department.name = name;
            dbContext.Departments.Add(department);
            dbContext.SaveChanges();
            return View();
        }
        public IActionResult CreateHall()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHall(string id, int capacity)
        {
            /*
            if (id == null)
            {
                ViewBag.Message = "emptyId";
                return View();
            }
            else if (capacity<=50)
            {
                ViewBag.Message = "emptyCapacity";
                return View();
            }
            */
            Hall hall = new Hall();
            hall.id = id;
            hall.capacity = capacity;
            dbContext.Add(hall);
            dbContext.SaveChanges();
            return View();
        }
        public async Task<IActionResult> DeleteDepartment()
        {
            try
            {
                // Retrieve all records from the table
                var recordsToDelete = dbContext.Departments.ToList();

                // Remove all records from the DbSet
                dbContext.Departments.RemoveRange(recordsToDelete);

                // Save changes to the database
                dbContext.SaveChanges();

                return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display an error message, etc.)
                return RedirectToAction("Error", "Home"); // Redirect to an error page
            }
        }

    
        public async Task<IActionResult> DeleteStudent()
        {
            try
            {
                // Retrieve all records from the table
                var recordsToDelete = dbContext.Students.ToList();

                // Remove all records from the DbSet
                dbContext.Students.RemoveRange(recordsToDelete);

                // Save changes to the database
                dbContext.SaveChanges();

                return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display an error message, etc.)
                return RedirectToAction("Error", "Home"); // Redirect to an error page
            }

        }
        public async Task<IActionResult> DeleteProfessor()
        {
            try
            {
                // Retrieve all records from the table
                var recordsToDelete = dbContext.Professors.ToList();

                // Remove all records from the DbSet
                dbContext.Professors.RemoveRange(recordsToDelete);

                // Save changes to the database
                dbContext.SaveChanges();

                return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display an error message, etc.)
                return RedirectToAction("Error", "Home"); // Redirect to an error page
            }

        }
        public async Task<IActionResult> DeleteHall()
        {
            try
            {
                // Retrieve all records from the table
                var recordsToDelete = dbContext.Halls.ToList();

                // Remove all records from the DbSet
                dbContext.Halls.RemoveRange(recordsToDelete);

                // Save changes to the database
                dbContext.SaveChanges();

                return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display an error message, etc.)
                return RedirectToAction("Error", "Home"); // Redirect to an error page
            }

        }
        public async Task<IActionResult> DeleteCourse()
        {
            try
            {
                // Retrieve all records from the table
                var recordsToDelete = dbContext.Courses.ToList();

                // Remove all records from the DbSet
                dbContext.Courses.RemoveRange(recordsToDelete);

                // Save changes to the database
                dbContext.SaveChanges();

                return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display an error message, etc.)
                return RedirectToAction("Error", "Home"); // Redirect to an error page
            }

        }
        public async Task<IActionResult> DeleteLecture()
        {
            
            try
            {
                // Retrieve all records from the table
                var recordsToDelete = dbContext.Lectures.ToList();
              
                // Remove all records from the DbSet
                dbContext.Lectures.RemoveRange(recordsToDelete);

                recordsToDelete = dbContext.Lectures.ToList();

                // Remove all records from the DbSet
                dbContext.Lectures.RemoveRange(recordsToDelete);

                // Save changes to the database
                dbContext.SaveChanges();

                return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display an error message, etc.)
                return RedirectToAction("Error", "Home"); // Redirect to an error page
            }

        }
        public async Task<IActionResult> Reset()
        {
            return View();
        }


    }
}