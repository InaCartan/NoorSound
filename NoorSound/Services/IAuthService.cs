using NoorSound.Models;
using Supabase;

namespace NoorSound.Services
{
    public interface IAuthService
    {
        Task SignUp(string email, string password, string adminName);
        Task LogIn(string email, string password);
        Task SignOut();
        Supabase.Gotrue.User? CurrentUser();
        string? CurrentUserId();
    }
}
