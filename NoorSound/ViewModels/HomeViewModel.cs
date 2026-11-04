// using NoorSound.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using NoorSound.Services;
using NoorSound.Services.Interfaces;
using NoorSound.Views;
using System.Collections.ObjectModel;

namespace NoorSound.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        // In Shaa Allah, ObservableCollection is used to update the ui if a change happens
        // (gives notification when items get added or removed). 
        public ObservableCollection<Audio> Audios { get; set; } = new ObservableCollection<Audio>();

        // The 4 newest audios (the small tiles at the top)
        public ObservableCollection<Audio> LatestAudios { get; } = new();

        // All playlists (the "Playlists" shelf)
        public ObservableCollection<Playlist> Playlists { get; } = new();

        // True while the user pulls down to refresh (used by the RefreshView in HomePage.xaml)
        [ObservableProperty]
        public partial bool IsRefreshing { get; set; }



        public HomeViewModel(
            IDataService dataService, 
            IDialogService dialogService, 
            INavigationService navigationService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _navigationService = navigationService;

        }

        [RelayCommand]
        public async Task LoadHome()
        {
            try
            {
                // Fetch everything first, so a failed load doesn't wipe what is already on screen
                var audios = (await _dataService.GetAudios()).ToList();
                var playlists = (await _dataService.GetPlaylists()).ToList();

                Audios.Clear();
                LatestAudios.Clear();
                Playlists.Clear();

                foreach (var audio in audios)
                {
                    Audios.Add(audio);
                }

                // GetAudios is already sorted newest first
                foreach (var audio in audios.Take(4))
                {
                    LatestAudios.Add(audio);
                }

                foreach (var playlist in playlists)
                {
                    Playlists.Add(playlist);
                }
            }
            catch
            {
                await _dialogService.ShowAlert("Error", "Unable to load audios, try to refresh page");
            }
        }


        // Pull-to-refresh
        [RelayCommand]
        private async Task Refresh()
        {
            try
            {
                await LoadHome();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        // In Shaa Allah ta'ala, this func navigates to PlaylistPage (same like in LibraryViewModel)
        [RelayCommand]
        private async Task OpenPlaylist(Playlist? playlist)
        {
            if (playlist is null) return;

            await _navigationService.GoToAsyncWithObject(
                nameof(PlaylistPage),
                new Dictionary<string, object>
                {
                    ["playlistId"] = playlist.Id,
                    ["playlistName"] = playlist.PlaylistName
                });
        }



        // In Shaa Allah ta'ala, this func navigates to AudioPlayerPage
        [RelayCommand]
        private async Task NavAudioPlayerPage(Audio audio)
        {
            await _navigationService.GoToAsyncWithObject(
               AppRoutes.AudioPlayer,
               new Dictionary<string, object> { { "Audio", audio } });
        }

        // In Shaa Allah ta'ala, this func navigates to ProfilePage
        [RelayCommand]
        private async Task NavProfilePage()
        {
            await _navigationService.GoToAsync(AppRoutes.Profile);
        }
    }
}

