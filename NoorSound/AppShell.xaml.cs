using NoorSound.Services;
using NoorSound.Views;

namespace NoorSound
{
    public partial class AppShell : Shell
    {

        private readonly IAuthService _authService;
        private bool _initialized;

        public AppShell(IAuthService authService)
        {
            InitializeComponent();

            _authService = authService;

            Routing.RegisterRoute(AppRoutes.AddAudio, typeof(AddAudioPage));

            // Loaded += async (_, __) => await InitializeAsync();
            Loaded += OnLoaded;

        }

        private async void OnLoaded(object? sender, EventArgs e)
        {
            if (_initialized)
                return;

            _initialized = true;

            await InitializeAsync();
        }

        private async Task InitializeAsync()
        {

            try
            {
                
                if (_authService.CurrentUser() == null)
                {
                    FlyoutBehavior = FlyoutBehavior.Disabled;
                    await GoToAsync("//Login");
                }
                else
                {
                    FlyoutBehavior = FlyoutBehavior.Flyout;
                    await GoToAsync("//Home");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }


        



    }
}
