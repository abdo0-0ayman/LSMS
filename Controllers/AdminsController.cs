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

namespace LSMS.Controllers
{
	[CustomAuthorize("Admins")]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	public class AdminsController : Controller
    {
        private readonly Services.IAuthenticationService authService;
        private readonly ApplicationDbContext dbContext;

        public AdminsController(Services.IAuthenticationService authService ,ApplicationDbContext dbContext)
        {
            this.authService = authService;
            this.dbContext = dbContext;
        }


		[CustomAuthorize("Admins")]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]


        public IActionResult Profile()
        {
			string username = User.Identity.Name;

			// Retrieve the full professor details from the database using dbContext
			var loggedIn = dbContext.Admins.FirstOrDefault(p => p.UserName == username);
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
                        var users= new List<User>();
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

                                var student = new Student
                                {
                                    Name = reader.GetValue(0).ToString(),
                                    SSN = reader.GetValue(1).ToString(),
                                    PhoneNumber = reader.GetValue(2).ToString(),
                                    AcademicEmail = reader.GetValue(3).ToString(),
                                    Password = reader.GetValue(4).ToString(),
									DepartmentId = (reader.GetValue(5).ToString()),
									// Add other properties as needed
								};
                                var user = new User
                                {
                                    Username = reader.GetValue(1).ToString(),
                                    Password = reader.GetValue(4).ToString(),
                                    Role = "Students",
                                };
                                students.Add(student);
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

								var professor = new Professor
								{
									Name = reader.GetValue(0).ToString(),
									SSN = reader.GetValue(1).ToString(),
									PhoneNumber = reader.GetValue(2).ToString(),
									Password = reader.GetValue(3).ToString(),
									DepartmentId= (reader.GetValue(4).ToString()),
									// Add other properties as needed
								};
                                var user = new User
								{
									Username = reader.GetValue(1).ToString(),
									Password = reader.GetValue(3).ToString(),
									Role = "Professors",
								};
								professors.Add(professor);
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

        public ActionResult StudentsEnrolled()
        {
            List<Student> student = dbContext.Students.Include(s => s.Department).ToList();
            if (student.Count() != 0)
            {
                return View(student);
            }
            ViewBag.ErrorMessage = "there is no students";
            return View(student);
        }
    }
}
