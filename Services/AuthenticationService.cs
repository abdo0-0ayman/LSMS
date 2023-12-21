using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace LSMS.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext dbContext;
        private Professor? loggedInProfessor;
        private Admin? loggedInAdmin;
        private Student? loggedInStudent;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public Professor? AuthenticateProfessor(string username, string password)
        {
            loggedInProfessor = dbContext.Professors.FirstOrDefault(p => p.SSN == username && p.password == password);

            return loggedInProfessor;
        }
        public Student? AuthenticateStudent(string username, string password)
        {
            loggedInStudent = dbContext.Students.FirstOrDefault(p => p.SSN == username && p.password == password);

            return loggedInStudent;
        }
        public Admin? AuthenticateAdmin(string username, string password)
        {
            loggedInAdmin = dbContext.Admins.FirstOrDefault(p => p.userName == username && p.password == password);

            return loggedInAdmin;
        }
        public void SignInProfessor(Professor professor)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, professor.SSN),
                // Add other claims as needed
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                IsPersistent = false,// or false, depending on your requirements
                                     // Other properties...
            };
            httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public void SignInStudent(Student student)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, student.SSN),
                // Add other claims as needed
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = false,// or false, depending on your requirements
                                     // Other properties...
            };
            httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public void SignInAdmin(Admin admin)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.userName),
                // Add other claims as needed
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = false,// or false, depending on your requirements
                                     // Other properties...
            };
            httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        public void SignOut()
        {
            httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }
}
