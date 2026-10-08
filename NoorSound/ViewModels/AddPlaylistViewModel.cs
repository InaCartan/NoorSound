using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using System.Collections.ObjectModel;
using NoorSound.Services.Interfaces;

namespace NoorSound.ViewModels
{
    public partial class AddPlaylistViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;


        // In Shaa Allah ta'ala, ObservableCollection is used to update the ui if a change happens
        // (gives notification when items get added or removed). 
        public ObservableCollection<Audio> Audios { get; set; } = new ObservableCollection<Audio>();

        [ObservableProperty]
        public partial string NewPlaylistName { get; set; } = string.Empty;

           
        public AddPlaylistViewModel(
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
        private async Task AddPlaylist()
        {
            if (string.IsNullOrWhiteSpace(NewPlaylistName))
            {
                await _dialogService.ShowAlert(
                    "Whoops...",
                    "Remember to name your playlist"
                    );
            }

            var userId = _authService.CurrentUserId();
            if (userId == null)
            {
                await _dialogService.ShowAlert(
                    "Sorry...",
                    "You need an account to make a playlist. Sign up, it's free!"
                    );
            }

            try
            {
                var newPlaylist = new PlaylistInsert
                {
                    PlaylistName = NewPlaylistName,
                    AdminId = userId ?? string.Empty
                };

                await _dataService.AddPlaylist(newPlaylist);

                foreach (Audio audio in Audios)
                {
                    var playlistAudio = new PlaylistAudioInsert
                    {
                        PlaylistId = newPlaylist.AdminId,
                        AudioId = audio.Id
                    };
                    await _dataService.AddAudioToPlaylist(playlistAudio);

                    // ** (the following comment is "almost" auto generated) **
                    // Navigate back to the previous page (PlaylistViewModel) just like pressing the back button 
                    await _navigationService.GoBackAsync();
                }
                
            }
            catch
            {

                await _dialogService.ShowAlert("Error", "Something didn't work, try again");
                return;
            }
            
        }


       



    }
}
