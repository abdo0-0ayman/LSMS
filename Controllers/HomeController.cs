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

			var user = dbContext.Users.FirstOrDefault(p => p.userName == username && p.password == password);
			if (user != null)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.userName),
					new Claim(ClaimTypes.Role, user.role)
					// Add other claims as needed
				};

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var principal = new ClaimsPrincipal(identity);
				await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

				return RedirectToAction("Profile", user.role);
			}
			ViewBag.ErrorMessage = "Invalid username or password";
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
