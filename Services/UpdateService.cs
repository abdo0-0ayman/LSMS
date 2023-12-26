using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection.PortableExecutable;
using System.Text;

namespace LSMS.Services
{
    public class UpdateService: IUpdateService
    {
        private readonly ApplicationDbContext dbContext;

        public UpdateService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public void ResetPassword(string username, string password)
        {
            var oldUser=dbContext.Users.FirstOrDefault(e=>e.userName==username);
            
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            oldUser.Salt = Encoding.UTF8.GetBytes(salt);
            oldUser.PasswordHash = Encoding.UTF8.GetBytes(hashedPassword);
            dbContext.SaveChanges();
        }
        public void UpdateStudent(EditModel student)
        {
            var oldStudent=dbContext.Students.FirstOrDefault(s=>s.SSN== student.SSN);
            oldStudent.name = student.name;
            oldStudent.phoneNumber=student.phoneNumber;
            dbContext.SaveChanges();
        }
        public void UpdateProfessor(EditModel professor)
        {
            var oldProfessor = dbContext.Professors.FirstOrDefault(s => s.SSN == professor.SSN);
            oldProfessor.name = professor.name;
            oldProfessor.phoneNumber = professor.phoneNumber;
            dbContext.SaveChanges();
        }
    }
}
