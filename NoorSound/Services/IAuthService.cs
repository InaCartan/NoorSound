using NoorSound.Models;

namespace NoorSound.Services
{
    public interface IAuthService
    {
        Task SignUp(string email, string password, string adminName);
        Task LogIn(string email, string password);
        Task SignOut();
        string CurrentUserId();
    }
}
