// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using NoorSound.Models;
using NoorSound.Services.Interfaces;
using Supabase;


namespace NoorSound.Services
{
    public class AuthService : IAuthService
    {
        private readonly Client _supabaseClient;


        public AuthService(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }


        public async Task SignUp(string email, string password, string adminName)
        {
            var options = new Supabase.Gotrue.SignUpOptions
            {
                Data = new Dictionary<string, object>
                {
                    ["admin_name"] = adminName.Trim()
                }
            };

            var response = await _supabaseClient.Auth.SignUp(
                email.Trim(),
                password,
                options);

            if (response?.User == null ||
                string.IsNullOrWhiteSpace(response.User.Id))
            {
                throw new InvalidOperationException(
                    "Signup failed: no valid user was returned.");
            }
        }

        public async Task LogIn(string email, string password)
        {
            //if an exception is thrown, viewModel will In Shaa Allah ta'ala catch it. 
            await _supabaseClient.Auth.SignIn(email, password); 
        }

        public async Task SignOut()
        {
            await _supabaseClient.Auth.SignOut();
        }

        public Supabase.Gotrue.User? CurrentUser()
        {
            // ** (almost) auto generated comment **
            // In Shaa Allah ta'ala, this return the current user, if there is no user, it will return null 
            return _supabaseClient.Auth.CurrentUser;
        }


        public string? CurrentUserId()
        {
            // ** (almost) auto generated comment **
            // In Shaa Allah ta'ala, this return the current user id, if there is no user, it will return null 
            return _supabaseClient.Auth.CurrentUser?.Id; 
        }

        // ** TODO: Check if all those who use CurrentUser should be replaced with this method.
        public async Task<string?> CurrentUserIdAsync()
        {
            return _supabaseClient.Auth.CurrentUser?.Id; 
        }
    }
}
