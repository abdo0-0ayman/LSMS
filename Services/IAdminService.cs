using LSMS.Models;

namespace LSMS.Services
{
    public interface IAdminService
    {
        void AddProfessor(Professor professor);
        void RemoveProfessor(string professorId);
        Professor GetProfessor(string professorId);
        Professor UpdateProfessor(string professorId, Professor professor);    

        void AddStudent(Student student);
        void RemoveStudent(string studentId);
        Student GetStudent(string studentId);
        Student UpdateStudent(Student student);

    }
}
