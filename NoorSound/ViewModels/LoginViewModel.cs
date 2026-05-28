using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using NoorSound.Services;
using NoorSound.Views;

namespace NoorSound.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string adminName;

        public LoginViewModel(IAuthService authService, IServiceProvider serviceProvider)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        private async Task Login()
        {
            try
            {
                await _authService.SignIn(Email, Password);

                // In Shaa Allah, switch app into main page (AppShell) after successful login
                App.Current.Windows[0].Page =
                     _serviceProvider.GetRequiredService<AppShell>();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    ex.Message,
                    "OK");
            }
        }

        [RelayCommand]
        private async Task SignUpRedirect()
        {
            // In Shaa Allah, when not signing in, the user is outside the shell. (see navigation stack - i think?)
            var page = _serviceProvider.GetRequiredService<SignUpPage>();
            App.Current.Windows[0].Page = page;
        }

        [RelayCommand]
        private async Task SignUp()
        {
            try
            {
                await _authService.SignUp(Email, Password, AdminName);

                App.Current.Windows[0].Page =
                    _serviceProvider.GetRequiredService<AppShell>();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    ex.Message,
                    "OK");
            }
        }
    }
}
