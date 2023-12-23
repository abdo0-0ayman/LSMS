using LSMS.data_access;
using LSMS.Models;

namespace LSMS.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddProfessor(Professor professor)
        {
            _context.Professors.Add(professor);
            _context.SaveChanges();
        }

        public void AddStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
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
            var result = _context.Professors.FirstOrDefault(n => n.SSN == professorId);
            _context.Professors.Remove(result);
            _context.SaveChanges();
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
