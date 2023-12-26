using LSMS.Models;

namespace LSMS.Services
{
    public interface IUpdateService
    {
        void UpdateProfessor(EditModel professor);
        void UpdateStudent(EditModel student);
        void ResetPassword(string username, string password);

    }
}
