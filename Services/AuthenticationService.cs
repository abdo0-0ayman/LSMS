using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Authentication;
using System;

namespace LSMS.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext dbContext;
        private Professor loggedInProfessor;

        public AuthenticationService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Professor Authenticate(string username, string password)
        {
            loggedInProfessor = dbContext.Professors.FirstOrDefault(p => p.SSN == username && p.Password == password);

            return loggedInProfessor;
        }
        public void Logout()
        {
            loggedInProfessor = null;
            // Clear any other data related to authentication if needed
        }
    }
}
