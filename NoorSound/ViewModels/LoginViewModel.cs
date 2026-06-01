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

                NavigateTo<AppShell>();

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
            NavigateTo<SignUpPage>();

        }

        [RelayCommand]
        private async Task SignUp()
        {
            try
            {
                await _authService.SignUp(Email, Password, AdminName);

                NavigateTo<AppShell>();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    ex.Message,
                    "OK");
            }
        }

        private void NavigateTo<TPage>()
            where TPage : Page
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var window = App.Current?.Windows?.FirstOrDefault();

                if (window == null)
                    throw new Exception("No application window found.");

                window.Page =
                    _serviceProvider.GetRequiredService<TPage>();
            });
        }
    }
}
