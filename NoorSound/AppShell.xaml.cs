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

            Routing.RegisterRoute(AppRoutes.AddAudio, typeof(AddAudioPage));

            

        }

        

        public async Task InitializeAsync()
        {
    
            if (_authService.CurrentUser() == null)
            {
                FlyoutBehavior = FlyoutBehavior.Disabled;
                await GoToAsync(AppRoutes.Login); 
            }
            else
            {
                FlyoutBehavior = FlyoutBehavior.Flyout;
                await GoToAsync(AppRoutes.Home);
            }
            
        }
    }
}
