using NoorSound.Services;
using NoorSound.Views;

namespace NoorSound
{
    public partial class AppShell : Shell
    {

        private readonly IAuthService _authService;

        public AppShell(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            Loaded += OnLoaded;

           
        }

        private async void OnLoaded(object? sender, EventArgs e)
        {
            if (_authService.CurrentUser() == null)
            {
                await GoToAsync("//LoginPage");

                Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            }
            else
            {
                await GoToAsync("//HomePage");

                Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            }
        }

        

    }
}
