using LSMS.Models;

namespace LSMS.Services
{
    public interface IStudentService
    {
        Student GetStudent(string id);
        void Add(Student student);
        Student Update(string id,Student student);
        void Delete(string id);

    }
}
