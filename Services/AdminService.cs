using LSMS.data_access;
using LSMS.Models;

namespace LSMS.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext dbContext;

        public AdminService(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public void AddProfessor(Professor professor)
        {
            dbContext.Professors.Add(professor);
            dbContext.SaveChanges();
        }

        public void AddStudent(Student student)
        {
            dbContext.Students.Add(student);
            dbContext.SaveChanges();
        }

        public Professor GetProfessor(string professorId)
        {
            throw new NotImplementedException();
        }

        public Student GetStudent(string studentId)
        {
            throw new NotImplementedException();
        }

        public void RemoveProfessor(string professorId)
        {
            var result = dbContext.Professors.FirstOrDefault(n => n.SSN == professorId);
            dbContext.Professors.Remove(result);
            dbContext.SaveChanges();
        }

        public void RemoveStudent(string studentId)
        {
            throw new NotImplementedException();
        }

        public Professor UpdateProfessor(string professorId, Professor professor)
        {
            throw new NotImplementedException();
        }

        public Student UpdateStudent(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
