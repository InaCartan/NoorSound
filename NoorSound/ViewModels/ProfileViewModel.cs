using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Services;
using NoorSound.Models;
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
        public partial string Password { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string AdminName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string ErrorMessage { get; set; } = string.Empty;

        public ProfileViewModel(IDataService dataService, IAuthService authService) {
            _authService = authService;
            _dataService = dataService;
        }



       
    }
}
