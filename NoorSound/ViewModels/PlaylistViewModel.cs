using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using NoorSound.Services.Interfaces;
using System.Collections.ObjectModel;

namespace NoorSound.ViewModels
{
    
    public partial class PlaylistViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IDataService _dataService;
        private readonly IAuthService _authService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<Audio> Audios { get; set; } = new ObservableCollection<Audio>();

        [ObservableProperty]
        public partial string PlaylistName { get; set; } = string.Empty;

        // Which playlist to show (passed in when navigating to this page)
        [ObservableProperty]
        public partial long PlaylistId { get; set; }

        public PlaylistViewModel(
            IDataService dataService,
            IAuthService authService,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            _dataService = dataService;
            _authService = authService;
            _dialogService = dialogService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        public async Task LoadPlaylistAudios()
        {
            try
            {
                // Fetch first, so a failed load doesn't wipe the list
                var audios = await _dataService.GetAudiosInPlaylist(PlaylistId);

                Audios.Clear();
                foreach (var audio in audios)
                {
                    Audios.Add(audio);
                }
            }
            catch
            {
                await _dialogService.ShowAlert("Error", "Something didn't work, try again");
            }
        }

        

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("playlistId", out var id) &&
                long.TryParse(id?.ToString(), out var playlistId))
            {
                PlaylistId = playlistId;
            }

            if (query.TryGetValue("playlistName", out var name))
            {
                PlaylistName = name?.ToString() ?? string.Empty;
            }
        }
    }
}