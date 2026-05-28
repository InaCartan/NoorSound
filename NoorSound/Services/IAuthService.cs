using NoorSound.Models;

namespace NoorSound.Services
{
    public interface IAuthService
    {
        Task SignUp(string email, string password, string adminName);
        Task SignIn(string email, string password);
        Task SignOut();
        string CurrentUserId();
    }
}
