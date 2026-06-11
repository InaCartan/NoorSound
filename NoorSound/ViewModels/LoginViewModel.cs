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

        [ObservableProperty]
        private string errorMessage;

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
            catch
            {
                await ShowError("Something went wrong, did you type a valid email or password?");
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
                await ShowError("Something went wrong, have you already signed up?");
            }
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
