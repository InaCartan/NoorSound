using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using System.Collections.ObjectModel;
using NoorSound.Services.Interfaces;

namespace NoorSound.ViewModels
{
    public partial class AddPlaylistViewModel
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
            //    // Image and Audio files may be uploaded to Supabase storage, while the nothing has been added to audio table in database.
            //    // In Shaa Allah ta'ala, thus these two files need to be deleted from Supabase storage.
            //    if (!string.IsNullOrWhiteSpace(NewPlaylistName))
            //    {
            //        await _dataService.DeleteFileFromStorage("playlists", NewPlaylistName);
            //    }

                await _dialogService.ShowAlert("Error", "Something didn't work, try again");
                return;
            }
            
        }


        [RelayCommand]
        private async Task AddPlaylist1()
        {
            if (string.IsNullOrWhiteSpace(NewPlaylistName))
            {
                await _dialogService.ShowAlertAsync("Error", "Please enter a playlist name.", "OK");
                return;
            }
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                await _dialogService.ShowAlertAsync("Error", "You must be logged in to make a playlist.", "OK");
                return;
            }
            var newPlaylist = new Playlist
            {
                PlaylistName = NewPlaylistName,
                AdminId = currentUser.Id
            };
            var makePlaylist = await _dataService.AddPlaylist(newPlaylist);
            if (makePlaylist != null)
            {
                // Add selected audios to the playlist
                foreach (var audio in Audios)
                {
                    var playlistAudio = new PlaylistAudio
                    {
                        PlaylistId = makePlaylist.Id,
                        audioId = audio.Id
                    };
                    await _dataService.AddAudioToPlaylistAsync(playlistAudio);
                }
                await _dialogService.ShowAlertAsync("Success", "Playlist made successfully!", "OK");
                await _navigationService.GoBackAsync();
            }
            else
            {
                await _dialogService.ShowAlertAsync("Error", "Failed to make playlist. Please try again.", "OK");
            }

        }




    }
}
