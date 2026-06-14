using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Services;
using NoorSound.Views;

namespace NoorSound.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider; 

        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Password { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string AdminName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string ErrorMessage { get; set; } = string.Empty;

        public LoginViewModel(IAuthService authService, IServiceProvider serviceProvider)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        private async Task LogIn()
        {
            try
            {
                await _authService.LogIn(Email, Password);

                NavigateTo<AppShell>();
            }
            catch
            {
                await ShowError("Email or password is wrong.");
            }
        }


        [RelayCommand]
        private async Task SignUp()
        {
            try
            {
                await _authService.SignUp(Email, Password, AdminName);

                NavigateTo<AppShell>();
            }
            catch(Exception ex)
            {
                await ShowError(ex.Message);
            }
        }



        [RelayCommand]
        private async Task SignUpRedirect()
        {
            NavigateTo<SignUpPage>();
        }

        [RelayCommand]
        private async Task LogInRedirect()
        {
            NavigateTo<LoginPage>();
        }



        private void NavigateTo<TPage>() where TPage : Page
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var window = Application.Current?.Windows?.FirstOrDefault();

                if (window == null)
                    return;

                window.Page = _serviceProvider.GetRequiredService<TPage>();
            });
        }

        private async Task ShowError(string message)
        {
            var page = Application.Current?.Windows?.FirstOrDefault()?.Page as Page;

            if (page != null)
            {
                await page.DisplayAlertAsync("Whoops", message, "OK");
            }
        }
    }
}
