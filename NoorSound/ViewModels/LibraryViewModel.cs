using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Services;
using NoorSound.Models;
using System.Collections.ObjectModel;

namespace NoorSound.ViewModels
{
    public partial class LibraryViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;


        // In Shaa Allah, ObservableCollection is used to update the ui if a change happens
        // (gives notification when items get added or removed). 
        public ObservableCollection<Audio> Audios { get; set; } = new();


        public LibraryViewModel(
            IDataService dataService, 
            IAuthService authService, 
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _dataService = dataService;
            _authService = authService;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }


        [RelayCommand]
        public async Task LoadAudios()
        {
            try
            {
                Audios.Clear(); 

                var userId = _authService.CurrentUserId();

                var allAudios = await _dataService.GetAudios();

                var myAudios = allAudios.Where(a => a.AdminId == userId);

                if (myAudios.Any())
                {
                    foreach (var audio in myAudios)
                    {
                        Audios.Add(audio); 
                    }
                }
            }

            catch
            {
                await _dialogService.ShowAlert("Error", "Unable to load audios");
            }
        }




        // In Shaa Allah, this func navigates to AddAudioPage.xaml.cs (not the viewmodel)
        [RelayCommand]
        private async Task AddAudio()
        {
            await _navigationService.GoToAsync("AddAudioPage");
        }



        [RelayCommand]
        private async Task DeleteAudio(Audio audio)
        {
            bool confirmed = await _dialogService.ShowConfirmation(
             "Delete",
             $"Are you sure you want to remove \"{audio.AudioName}\"?");


            if (confirmed)
            {
                try
                {
                    await _dataService.DeleteAudio(audio.Id);
                    await LoadAudios(); // In Shaa Allah, refresh the list after a deletion
                }
                catch 
                {
                    await _dialogService.ShowAlert("Error", "Unable to delete audio");
                }
            }

        }
    }
}

