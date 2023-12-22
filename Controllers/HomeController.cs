using LSMS.data_access;
using LSMS.Models;
using LSMS.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

namespace LSMS.Controllers
{

    public class HomeController : Controller
    {
		private readonly ApplicationDbContext dbContext;
		private readonly IHttpContextAccessor httpContextAccessor;

		public HomeController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
		{
			this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
			this.dbContext = dbContext;
		}

		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
			{
				// Redirect to the home page or another secure page
				Console.WriteLine(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
				return RedirectToAction("Profile", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
			}
			return View();
		}

		[HttpPost]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public async Task<IActionResult> Login(string username, string password)
		{
			if (User.Identity.IsAuthenticated)
			{
				// Redirect to the home page or another secure page
				return RedirectToAction("Profile", User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
			}

			var user = dbContext.Users.FirstOrDefault(p => p.userName == username);
			if (user != null)
			{
                var hashedPassword = Encoding.UTF8.GetString(user.PasswordHash);
                var salt = Encoding.UTF8.GetString(user.Salt);

                if (!(BCrypt.Net.BCrypt.Verify(password, hashedPassword)))
                {
                    ViewBag.ErrorMessage = "You Entered An Invalid Password";
                    return View();
                }
                var loggedInProfessor = dbContext.Professors.FirstOrDefault(p => p.SSN == username);
                var loggedInStudent = dbContext.Students.FirstOrDefault(p => p.SSN == username);
                var loggedInAdmin = dbContext.Admins.FirstOrDefault(p => p.userName == username);
				string name = username;
				if(loggedInProfessor != null )name=loggedInProfessor.name; 
				if(loggedInStudent != null)name=loggedInStudent.name; 
				if(loggedInAdmin != null)name=loggedInAdmin.name;
                var claims = new List<Claim>
				{
                    new Claim(ClaimTypes.NameIdentifier , username),
                    new Claim(ClaimTypes.Name, name),
					new Claim(ClaimTypes.Role, user.role)
					// Add other claims as needed
				};

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var principal = new ClaimsPrincipal(identity);
				await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

				return RedirectToAction("Profile", user.role);
			}
			ViewBag.ErrorMessage = "You Entered An Invalid Username";
			return View();
		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		}
		public IActionResult Index()
        {

			return RedirectToAction("Login", "Home");
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
	}
}
