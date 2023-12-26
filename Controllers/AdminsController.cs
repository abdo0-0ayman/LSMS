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
using System.Collections.Specialized;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.Numerics;
using Microsoft.AspNetCore.Http;
using System.IO;

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


        public ActionResult ProfessorsEnrolled()
        {
            var professors = dbContext.Professors.Include(s => s.department).ToList();
            if (professors.Count() != 0)
            {
                return View(professors);
            }
            ViewBag.ErrorMessage = "there is no students";
            return View(professors);
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
                if (halls.Count() == 1) 
                    totalIntersections = 0;
                foreach (var dep in dbContext.Departments.Include(l => l.courses).ToList())
                {
                    if (dep.courses.Count() > maxCourses)
                    {
                        maxCourses = dep.courses.Count();
                    }
                }
                if (totalIntersections + maxCourses > 25)
                {
                    ViewBag.ErrorMessage = "We can't generate schedule because you exceeded the intersection limit";
                    return View();
                }
                else scheduleGeneratorService.GenerateScheduleBacktrack(lectures, halls);
            }
            return RedirectToAction("schedule");
        }
        private bool checkForSchedule()
        {
            var lectures = dbContext.Lectures.Where(e => e.hallId == null).Select(x => x).ToList();
            if (lectures.Count() != 0||!dbContext.Lectures.Any())
            {
                return true;
            }
            return false;
        }
        public IActionResult schedule()
        {
            
            var lectures = new List<Lecture>();
            if (checkForSchedule())
            {
                ViewBag.ErrorMessage = "The schedule not generated yet";
                ViewBag.Halls=new List<Hall>();
                return View(lectures);
            }
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
            var departments = dbContext.Departments.ToList();
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CreateDepartment department1)
        {
            var departments=dbContext.Departments.ToList();
            ViewBag.Departments = departments;
            /*
            if (id == null)
            {
                ViewBag.ErrorMessage = "emptyId";
                return View();
            }
            else if (name == null)
            {
                ViewBag.ErrorMessage = "emptyName";
                return View();
            }
            */
            if (!ModelState.IsValid)
            {
                return View(department1);
            }
            var checkForDepartmnet = dbContext.Departments.Where(l => l.id == department1.id);
            if(checkForDepartmnet.Any())
            {
                ViewBag.ErrorMessage = "This department is already exist";
                return View();
            }
            Department department = new Department();
            department.id = department1.id;
            department.name = department1.name;
            dbContext.Departments.Add(department);
            dbContext.SaveChanges();
            ViewBag.Message = "success";
            return View();
        }
        public IActionResult CreateHall()
        {
            var halls = dbContext.Halls.ToList();
            ViewBag.Halls = halls;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHall(CreateHall hall1)
        {
            var halls = dbContext.Halls.ToList();
            ViewBag.Halls = halls;

            /*
            if (id == null)
            {
                ViewBag.ErrorMessage = "emptyId";
                return View();
            }
            else if (capacity<=50)
            {
                ViewBag.ErrorMessage = "emptyCapacity";
                return View();
            }
            */
            if (!ModelState.IsValid)
            {
                return View(hall1);
            }
            var checkForHall = dbContext.Halls.Where(l => l.id == hall1.id);

            if (checkForHall.Any())
            {
                ViewBag.ErrorMessage = "This hall is already exist";
                return View();
            }
            Hall hall = new Hall();
            hall.id = hall1.id;
            hall.capacity = hall1.capacity;
            dbContext.Add(hall);
            dbContext.SaveChanges();
            ViewBag.Message = "success";
            return View();
        }

        public IActionResult CreateProfessor()
        {
            var departments = dbContext.Departments.ToList();
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfessor(CreateProfessor professor)
        {
            var departments = dbContext.Departments.ToList();
            ViewBag.Departments = departments;

            /*
            if (professor.SSN == null)
            {
                ViewBag.ErrorMessage = "emptyId";
                return View();
            }
            */
            if (!ModelState.IsValid)
            {
                return View(professor);
            }    
            var checkForProfessor = dbContext.Professors.Where(l => l.SSN == professor.SSN);
            if (checkForProfessor.Any())
            {
                ViewBag.ErrorMessage = "This Professor is already exist";
                return View();
            }
            Professor professor1 = new Professor();
            professor1.SSN = professor.SSN;
            professor1.phoneNumber = professor.phoneNumber;
            professor1.departmentId = professor.departmentId;
            professor1.name = professor.name;
            dbContext.Add(professor1);
            dbContext.SaveChanges();
            ViewBag.Message = "success";
            return View();
        }

        public IActionResult CreateStudent()
        {
            var departments = dbContext.Departments.ToList();
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudent student1 )
        {
            var departments = dbContext.Departments.ToList();
            ViewBag.Departments = departments;

            /*
            if (SSN.Length!=16)
            {
                ModelState.AddModelError("SSN", "Please Enter a valid SSN");
                return View();
            }
            */
            /*
            if (SSN == null)
            {
                ViewBag.ErrorMessage = "emptyId";
                return View();
            }
            */
            if (!ModelState.IsValid)
            {
                return View(student1);
            }

            var checkForStudent = dbContext.Students.Where(l => l.SSN == student1.SSN);
            if (checkForStudent.Any())
            {
                ViewBag.ErrorMessage = "This Student is already exist";
                return View();
            }
            Student student= new Student();
            student.SSN = student1.SSN;
            student.phoneNumber = student1.phoneNumber;
            student.academicEmail = student1.academicEmail;
            student.departmentId = student1.departmentId;
            student.name = student1.name;
            dbContext.Add(student);
            dbContext.SaveChanges();
            ViewBag.Message = "success";
            return View();
        }

        public IActionResult CreateCourse()
        {
            var courses = dbContext.Courses.ToList();
            ViewBag.Courses = courses;
            var departments = dbContext.Departments.ToList();
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourse course1)
        {
            var courses = dbContext.Courses.ToList();
            ViewBag.Courses = courses;
            var departments = dbContext.Departments.ToList();
            ViewBag.Departments = departments;
            /*
            if (id == null)
            {
                ViewBag.ErrorMessage = "emptyId";
                return View();
            }
            */
            if (!ModelState.IsValid)
            {
                return View(course1);
            }

            var checkForCourse = dbContext.Courses.Where(l => l.id == course1.id);
            if (checkForCourse.Any())
            {
                ViewBag.ErrorMessage = "This Course is already exist";
                return View();
            }
            Course course = new Course();
            course.id = course1.id;
            course.name = course1.name;
            course.hours = course1.hours;
            course.departmentId = course1.departmentId;
            dbContext.Add(course);
            dbContext.SaveChanges();
            ViewBag.Message = "success";
            return View();
        }

        public ActionResult CourseView()
        {
            var course = dbContext.Courses.Include(s => s.department).ToList();
            if (course.Count() != 0)
            {
                return View(course);
            }
            ViewBag.ErrorMessage = "there is no students";
            return View(course);
        }

        //public async Task<IActionResult> DeleteDepartment()
        //{
        //    try
        //    {
        //        // Retrieve all records from the table
        //        var recordsToDelete = dbContext.Departments.ToList();

        //        // Remove all records from the DbSet
        //        dbContext.Departments.RemoveRange(recordsToDelete);

        //        // Save changes to the database
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (log, display an error message, etc.)
        //        return RedirectToAction("Error", "Home"); // Redirect to an error page
        //    }
        //}


        //public async Task<IActionResult> DeleteStudent()
        //{
        //    try
        //    {
        //        // Retrieve all records from the table
        //        var recordsToDelete = dbContext.Students.ToList();

        //        // Remove all records from the DbSet
        //        dbContext.Students.RemoveRange(recordsToDelete);

        //        // Save changes to the database
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (log, display an error message, etc.)
        //        return RedirectToAction("Error", "Home"); // Redirect to an error page
        //    }

        //}
        //public async Task<IActionResult> DeleteProfessor()
        //{
        //    try
        //    {
        //        // Retrieve all records from the table
        //        var recordsToDelete = dbContext.Professors.ToList();

        //        // Remove all records from the DbSet
        //        dbContext.Professors.RemoveRange(recordsToDelete);

        //        // Save changes to the database
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (log, display an error message, etc.)
        //        return RedirectToAction("Error", "Home"); // Redirect to an error page
        //    }

        //}
        //public async Task<IActionResult> DeleteHall()
        //{
        //    try
        //    {
        //        // Retrieve all records from the table
        //        var recordsToDelete = dbContext.Halls.ToList();

        //        // Remove all records from the DbSet
        //        dbContext.Halls.RemoveRange(recordsToDelete);

        //        // Save changes to the database
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (log, display an error message, etc.)
        //        return RedirectToAction("Error", "Home"); // Redirect to an error page
        //    }

        //}
        //public async Task<IActionResult> DeleteCourse()
        //{
        //    try
        //    {
        //        // Retrieve all records from the table
        //        var recordsToDelete = dbContext.Courses.ToList();

        //        // Remove all records from the DbSet
        //        dbContext.Courses.RemoveRange(recordsToDelete);

        //        // Save changes to the database
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (log, display an error message, etc.)
        //        return RedirectToAction("Error", "Home"); // Redirect to an error page
        //    }

        //}
        //public async Task<IActionResult> DeleteLecture()
        //{

        //    try
        //    {
        //        // Retrieve all records from the table
        //        var recordsToDelete = dbContext.Lectures.ToList();

        //        // Remove all records from the DbSet
        //        dbContext.Lectures.RemoveRange(recordsToDelete);

        //        recordsToDelete = dbContext.Lectures.ToList();

        //        // Remove all records from the DbSet
        //        dbContext.Lectures.RemoveRange(recordsToDelete);

        //        // Save changes to the database
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Index", "Home"); // Redirect to a success page or any other page
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (log, display an error message, etc.)
        //        return RedirectToAction("Error", "Home"); // Redirect to an error page
        //    }

        //}
        //public async Task<IActionResult> Reset()
        //{
        //    return View();
        //}


    }
}