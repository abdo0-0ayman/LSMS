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

        private readonly IAuthenticationService authService;
        private readonly ApplicationDbContext dbContext;

        public ProfessorsController(Services.IAuthenticationService authService, ApplicationDbContext dbContext)
        {
            this.authService = authService;
            this.dbContext = dbContext;
        }
		[CustomAuthorize("Professors")]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Profile()
		{
			string username = User.Identity.Name;
			// Retrieve the full professor details from the database using dbContext
			var loggedIn = dbContext.Professors.FirstOrDefault(p => p.SSN == username);
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
                                    Name = reader.GetValue(0).ToString(),
                                    SSN = reader.GetValue(1).ToString(),
                                    DepartmentId = (reader.GetValue(2).ToString()),
                                    // Add other properties as needed
                                };
                                var students = dbContext.Students.Find(student.SSN);
                                if(students!=null)
                                {
                                    dbContext.Enrollments.Add(new Enrollment { LectureId=LectureId,StudentSSN=students.SSN });
                                }
                            }
                        } while (reader.NextResult());
                        await dbContext.SaveChangesAsync();

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
