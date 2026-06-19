using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using NoorSound.Services;
using System.Collections.ObjectModel;


namespace NoorSound.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string AdminName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string ErrorMessage { get; set; } = string.Empty;



        public ProfileViewModel(IDataService dataService, IAuthService authService) {
            _authService = authService;
            _dataService = dataService;
        }


        [RelayCommand]
        private async Task LoadProfile()
        {
            // BismiIllah
            // Load the current user and save their info (Email, AdminName and so on)
            // If there is no info about the user, navigate to a
            // new ProfilePage that ask the user to registere to get more benifits (for example saving audios and so on)

        }
       
    }
}
