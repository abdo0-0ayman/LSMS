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
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Professor? AuthenticateProfessor(string username, string password)
        {
            loggedInProfessor = dbContext.Professors.FirstOrDefault(p => p.SSN == username && p.Password == password);

            return loggedInProfessor;
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

            httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        public void SignOutProfessor()
        {
            httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
