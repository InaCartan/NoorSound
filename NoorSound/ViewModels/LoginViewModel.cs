using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Services;


namespace NoorSound.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;


        // Input Fields
        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Password { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string AdminName { get; set; } = string.Empty;


        // Field erros
        [ObservableProperty] 
        public partial string EmailError { get; set; } = string.Empty;

        [ObservableProperty] 
        public partial string PasswordError { get; set; } = string.Empty;

        [ObservableProperty] 
        public partial string AdminNameError { get; set; } = string.Empty;

        //[ObservableProperty]
        //public partial string ErrorMessage { get; set; } = string.Empty;

        public LoginViewModel(IAuthService authService, INavigationService navigationService)
        {
            _authService = authService; 
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task LogIn()
        {

            ClearErrors();

            if (!ValidateLogin())
                return;

            try
            {
                await _authService.LogIn(Email, Password);
                await _navigationService.GoToAsync("//HomePage");
                
            }
            catch
            {
                // TODO: show specific error (network error, wrong login and so on...)
                PasswordError = "Invalid email or password, try again";
                
            }
        }


        [RelayCommand]
        private async Task SignUp()
        {

            ClearErrors();

            if (!ValidateSignUp())
                return;

            try
            {
                await _authService.SignUp(Email, Password, AdminName);

                await _navigationService.GoToAsync("//HomePage");
                
            }
            catch
            {
                // TODO: show specific error (network error, wrong login and so on...)
                EmailError = "Whoops! Something went wrong, try again";
                
            }
        }


        //-- REDIRECTS --

        [RelayCommand]
        private async Task SignUpRedirect()
        {
            await _navigationService.GoToAsync("//SignUpPage");
            // NavigateTo<SignUpPage>();

        }

        [RelayCommand]
        private async Task LogInRedirect()
        {
            await _navigationService.GoToAsync("//LoginPage");
            // NavigateTo<LoginPage>();
        }


        // -- VALIDATIONS --

        private bool ValidateLogin()
        {
            bool ok = true;

            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Email is required";
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Password is required";
                ok = false;
            }

            return ok;
        }

        private bool ValidateSignUp()
        {
            bool ok = true;

            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Email is required";
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(AdminName))
            {
                AdminNameError = "Admin name is required";
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Password is required";
                ok = false;
            }

            return ok;
        }

        private void ClearErrors()
        {
            EmailError = string.Empty;
            PasswordError = string.Empty;
            AdminNameError = string.Empty;
        }
    }
}

