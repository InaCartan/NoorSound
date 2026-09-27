using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using NoorSound.Services;
using NoorSound.Services.Interfaces;


namespace NoorSound.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly IDialogService _dialogService;
        private readonly IDataService _dataService;

        private readonly INavigationService _navigationService;

        [ObservableProperty]
        public partial string AdminName { get; set; } = string.Empty;

        public ProfileViewModel(
            IDialogService dialogService,
            IAuthService authService,
            IDataService dataService,
            INavigationService navigationService)
        {
            _authService = authService;
            _dialogService = dialogService;
            _dataService = dataService;
            _navigationService = navigationService;
        }


        [RelayCommand]
        public async Task LoadProfile()
        {
            // BismiIllah
            // Load the current user and save their info (Email, AdminName and so on)
            // If there is no info about the user, navigate to a
            // new ProfilePage that ask the user to registere to get more benifits (for example saving audios and so on)
            
            var user = _authService.CurrentUser();
            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(user.Id))
                {
                    AdminName = string.Empty;
                    return;
                }

                var admin = await _dataService.GetAdmin(user.Id);
                AdminName = admin?.Name ?? string.Empty;
            }
            else
            {
                await _dialogService.ShowAlert(
                   "Whoops...",
                   "You need to have an account to upload audios. It's free to sign up!"
                   );

                return;
            }
        }

        // In Shaa Allah ta'ala, this func navigates to AddAudioPage
        [RelayCommand]
        private async Task NavToAudioPage(Audio audio)
        {
            await _navigationService.GoToAsyncWithObject(
                AppRoutes.AddPlaylist,
                new Dictionary<string, object> { { "Audio", audio } });
        }

    }
}
