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
        private readonly ApplicationDbContext dbContext;
        private readonly IUpdateService updateService;


        public ProfessorsController(IAuthenticationService authService, ApplicationDbContext dbContext, IUpdateService updateService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.updateService = updateService;
        }

        [CustomAuthorize("Professors")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Profile()
        {
            ClaimsPrincipal user = HttpContext.User;
            string username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var loggedIn = dbContext.Professors.FirstOrDefault(p => p.SSN == username);

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
            ClaimsPrincipal user = HttpContext.User;
            string username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedIn = dbContext.Professors.FirstOrDefault(p => p.SSN == username);
            var courses = dbContext.Courses.ToList();
            ViewBag.Courses = courses;
            return View(loggedIn);
        }

        [HttpPost]
        public IActionResult SelectCourses(string professorSSN, List<string> selectedCourses)
        {
            try
            {
                Professor professor = dbContext.Professors.FirstOrDefault(p => p.SSN == professorSSN);

                if (professor != null && selectedCourses != null && selectedCourses.Any())
                {
                    int count = 100 + dbContext.Lectures.Count();

                    foreach (var course in selectedCourses)
                    {
                        count++;
                        Lecture lecture = new Lecture
                        {
                            professorSSN = professorSSN,
                            courseId = course,
                            id = count.ToString()
                        };
                        dbContext.Lectures.Add(lecture);
                    }

                    dbContext.SaveChanges();

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
            ClaimsPrincipal user = HttpContext.User;
            string username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            // Retrieve the full professor details from the database using dbContext
            var loggedIn = dbContext.Professors.FirstOrDefault(p => p.SSN == username);
            var lecture = dbContext.Lectures.Where(p => p.professorSSN == loggedIn.SSN).Include(p=>p.course).ToList();
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
                var intersections = dbContext.Intersections.ToList();
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
                                var studentFind = dbContext.Students.Find(student.SSN);
                                if(studentFind!=null)
                                {
                                    students.Add(studentFind);
                                    var intersection = new Intersection()
                                    {
                                        departmentId = studentFind.departmentId,
                                        lectureId = LectureId
                                    };
                                    var lecture1=dbContext.Lectures.FirstOrDefault(l=>l.id==LectureId);
                                    if (!intersections.Contains(intersection))
                                    {
                                        intersections.Add(intersection);
                                    }
                                }
                            }
                        } while (reader.NextResult());
                        // count all lectures in 
                        var countlectures = dbContext.Lectures.Count();

                        var countintersections=intersections.Count();
                        if(countlectures > countintersections*2)
                        {
                            ViewBag.Message = "You exceeded the intersections limit";
                            return View();
                        }
                        foreach(var student in students)
                        {
                            var check = dbContext.Enrollments.Where(e => e.studentSSN == student.SSN &&
                            e.lectureId == LectureId).ToList();
                            if (check.Count==0)
                            {
                                dbContext.Enrollments.Add(new Enrollment()
                                {
                                    studentSSN = student.SSN,
                                    lectureId = LectureId
                                });
                            }
                        }
                        await dbContext.SaveChangesAsync();

                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            ClaimsPrincipal user = HttpContext.User;
            string username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            // Retrieve the full professor details from the database using dbContext
            var loggedIn = dbContext.Professors.FirstOrDefault(p => p.SSN == username);
            var lecture = dbContext.Lectures.Where(p => p.professorSSN == loggedIn.SSN).Include(p => p.course).ToList();

            return View(lecture);
        }
        private bool checkForSchedule()
        {
            var lectures = dbContext.Lectures.Where(e => e.hallId == null).Select(x=>x).ToList();
            if (lectures.Count()==0)
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
                return View(lectures);
            }
            ClaimsPrincipal user = HttpContext.User;
            string username = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedIn = dbContext.Professors.FirstOrDefault(p => p.SSN == username);
            lectures = dbContext.Lectures.Where(e=>e.professorSSN == loggedIn.SSN).ToList(); // Implement GetLectures() to retrieve your lectures from the database or another source
            return View(lectures);
        }
        [HttpGet]
        public IActionResult editProfile()
        {
            // Retrieve the user's current profile information from the database
            // You may use the logged-in user's username or ID to fetch the user data
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var professor = dbContext.Professors.FirstOrDefault(p => p.SSN == username);
            var model = new EditModel()
            {
                name = professor.name,
                phoneNumber = professor.phoneNumber,
                SSN=professor.SSN,
                OldPassword = "00000000"
                ,
                ConfirmPassword = "00000000",
                NewPassword = "00000000"
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult editProfile (EditModel professor)
        {
            if(ModelState.IsValid) {
                updateService.UpdateProfessor(professor);
                return RedirectToAction("Profile");
            }

            return View(professor);
        }
        public IActionResult ChangePassword()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var professor = dbContext.Professors.FirstOrDefault(p => p.SSN == username);
            var model = new EditModel()
            {
                name = professor.name,
                phoneNumber = professor.phoneNumber,
                SSN = professor.SSN,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(EditModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the current user from the database
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = dbContext.Users.FirstOrDefault(p => p.userName == username);

                // Verify the old password
                var hashedOldPassword = Encoding.UTF8.GetString(user.PasswordHash);
                if (!(BCrypt.Net.BCrypt.Verify(model.OldPassword, hashedOldPassword)))
                {
                    ViewBag.ErrorMessage = "Incorrect old password.";
                    return View(model);
                }

                // Update the password
                updateService.ResetPassword(user.userName, model.NewPassword);

                // Redirect to the profile or another secure page
                return RedirectToAction("Profile", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
            }

            return View(model);
        }
    }
}

