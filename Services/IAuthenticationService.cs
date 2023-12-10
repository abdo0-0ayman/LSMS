using LSMS.Models;

namespace LSMS.Services
{
    public interface IAuthenticationService
    {
        Professor Authenticate(string username, string password);
        void Logout();
    }
}
