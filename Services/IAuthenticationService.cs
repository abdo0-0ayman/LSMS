using LSMS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace LSMS.Services
{
    public interface IAuthenticationService
    {
        Professor AuthenticateProfessor(string username, string password);
        Student AuthenticateStudent(string username, string password);

        public void SignInProfessor(Professor professor);
        public void SignInStudent(Student student);
        public void SignOutProfessor();
        public void SignOutStudent();
    }
}
