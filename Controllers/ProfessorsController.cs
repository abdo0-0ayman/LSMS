using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // For Session
using LSMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ExcelDataReader;
using System.Text;


namespace LSMS.Controllers
{
	[CustomAuthorize("Professors")]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	public class ProfessorsController : Controller
    {

        private readonly IAuthenticationService _authService;
        private readonly ApplicationDbContext _dbContext;

        public ProfessorsController(IAuthenticationService authService, ApplicationDbContext dbContext)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [CustomAuthorize("Professors")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Profile()
        {
            ClaimsPrincipal user = HttpContext.User;
            string username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var loggedIn = _dbContext.Professors.FirstOrDefault(p => p.SSN == username);

                if (loggedIn != null)
                {
                    return View(loggedIn);
                }

                ViewBag.ErrorMessage = "Professor not found";
                return RedirectToAction("Logout", "Home");
            }
            catch (Exception ex)
            {
                // Log the exception
                ViewBag.ErrorMessage = "An error occurred while processing the request";
                return RedirectToAction("Logout", "Home");
            }
        }

        public IActionResult SelectCourses()
        {
            string username = User.Identity.Name;
            var loggedIn = _dbContext.Professors.FirstOrDefault(p => p.SSN == username);
            var courses = _dbContext.Courses.ToList();
            ViewBag.Courses = courses;
            return View(loggedIn);
        }

        [HttpPost]
        public IActionResult SelectCourses(string professorSSN, List<string> selectedCourses)
        {
            try
            {
                Professor professor = _dbContext.Professors.FirstOrDefault(p => p.SSN == professorSSN);

                if (professor != null && selectedCourses != null && selectedCourses.Any())
                {
                    int count = 100 + _dbContext.Lectures.Count();

                    foreach (var course in selectedCourses)
                    {
                        count++;
                        Lecture lecture = new Lecture
                        {
                            professorSSN = professorSSN,
                            courseId = course,
                            id = count.ToString()
                        };
                        _dbContext.Lectures.Add(lecture);
                    }

                    _dbContext.SaveChanges();

                    return RedirectToAction("Profile");
                }

                ViewBag.ErrorMessage = "Invalid professor or selected courses";
                return RedirectToAction("Logout", "Home");
            }
            catch (Exception ex)
            {
                // Log the exception
                ViewBag.ErrorMessage = "An error occurred while processing the request";
                return RedirectToAction("Logout", "Home");
            }
        }


        public IActionResult UploadExcelStudent()
        {
            string username = User.Identity.Name;
            // Retrieve the full professor details from the database using dbContext
            var loggedIn = _dbContext.Professors.FirstOrDefault(p => p.SSN == username);
            var lecture = _dbContext.Lectures.Where(p => p.professorSSN == loggedIn.SSN).Include(p=>p.course).ToList();
            return View(lecture);
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelStudent(IFormFile file,string LectureId)
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
                int cnt = 0;
                var intersections = _dbContext.Intersections.ToList();
                List<Student> students = new List<Student>();
                // 
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var users = new List<User>();
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (reader.GetValue(0) == null) break;
                                cnt++;
                                Console.WriteLine(cnt);
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                var student = new Student
                                {
                                    name = reader.GetValue(0).ToString(),
                                    SSN = reader.GetValue(1).ToString(),
                                    departmentId = (reader.GetValue(2).ToString()),
                                    // Add other properties as needed
                                };
                                var studentFind = _dbContext.Students.Find(student.SSN);
                                if(studentFind!=null)
                                {
                                    students.Add(studentFind);
                                    var intersection = new Intersection()
                                    {
                                        departmentId = studentFind.departmentId,
                                        lectureId = LectureId
                                    };
                                    if (!intersections.Contains(intersection))
                                    {
                                        intersections.Add(intersection);
                                    }
                                }
                            }
                        } while (reader.NextResult());
                        // count all lectures in 
                        var countlectures = _dbContext.Lectures.Count();

                        var countintersections=intersections.Count();
                        if(countlectures > countintersections*2)
                        {
                            ViewBag.Message = "You exceeded the intersections limit";
                            return View();
                        }
                        foreach(var student in students)
                        {
                            _dbContext.Enrollments.Add(new Enrollment()
                            {
                                studentSSN= student.SSN,
                                lectureId=LectureId
                            });
                        }
                        await _dbContext.SaveChangesAsync();

                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
