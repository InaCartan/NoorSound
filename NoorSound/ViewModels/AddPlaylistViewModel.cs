using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using System.Collections.ObjectModel;
using NoorSound.Services.Interfaces;

namespace NoorSound.ViewModels
{
    public partial class SelectableAudio : ObservableObject
    {
        public SelectableAudio(Audio audio, bool isSelected)
        {
            Audio = audio;
            IsSelected = isSelected;
        }

        public Audio Audio { get; }

        [ObservableProperty]
        public partial bool IsSelected { get; set; }
    }


    public partial class AddPlaylistViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

      
        // In Shaa Allah ta'ala, ObservableCollection is used to update the ui if a change happens
        // (gives notification when items get added or removed). 
        public ObservableCollection<Audio> Audios { get; set; } = new ObservableCollection<Audio>();

        // The user's have audios. Every ticked one becomes a row in the playlist_audio junction table.
        public ObservableCollection<SelectableAudio> MyAudios { get; } = new();

        [ObservableProperty]
        public partial string NewPlaylistName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool HasNoAudios { get; set; }

        [ObservableProperty]
        public partial bool IsBusy { get; set; }

        // ----- Upload new audio form (similar to AddAudioViewModel) -----

        [ObservableProperty]
        public partial bool ShowUploadForm { get; set; }

        [ObservableProperty]
        public partial string NewAudioName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string NewImageUrl { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string NewImagePath { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string NewAudioUrl { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string NewAudioPath { get; set; } = string.Empty;


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

        // ------- Load the user's previously uploaded audios -------

        [RelayCommand]
        private async Task LoadMyAudios()
        {
            await RefreshMyAudios();
        }

        // Keeps whatever is already ticked, and optionally ticks one more (a freshly uploaded audio)
        private async Task RefreshMyAudios(long? alsoSelectId = null)
        {
            var userId = _authService.CurrentUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                MyAudios.Clear();
                HasNoAudios = true;
                return;
            }

            try
            {
                var selectedIds = MyAudios
                    .Where(a => a.IsSelected)
                    .Select(a => a.Audio.Id)
                    .ToHashSet();

                if (alsoSelectId.HasValue)
                {
                    selectedIds.Add(alsoSelectId.Value);
                }

                var audios = await _dataService.GetAudiosByAdmin(userId);

                MyAudios.Clear();
                foreach (var audio in audios)
                {
                    MyAudios.Add(new SelectableAudio(audio, selectedIds.Contains(audio.Id)));
                }

                HasNoAudios = MyAudios.Count == 0;
            }
            catch
            {
                await _dialogService.ShowAlert("Whoops...", "Unable to load your audios");
            }
        }

        // ------- Upload a new audio from inside the playlist page -------

        [RelayCommand]
        private void ToggleUploadForm()
        {
            ShowUploadForm = !ShowUploadForm;
        }


        [RelayCommand]
        private async Task PickAudio()
        {
            try
            {
                var audioTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.audio" } },
                    { DevicePlatform.Android, new[] { "audio/*" } },
                    { DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".m4a", ".aac" } },
                    { DevicePlatform.macOS, new[] { "public.audio" } }
                });

                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Audio",
                    FileTypes = audioTypes
                });

                if (result != null)
                {
                    IsBusy = true;

                    using var stream = await result.OpenReadAsync();

                    var uploadedAudio = await _dataService.UploadFile(stream, result.FileName, "audio-files");

                    NewAudioPath = uploadedAudio.Path;
                    NewAudioUrl = uploadedAudio.PublicUrl;
                }
            }
            catch
            {
                await _dialogService.ShowAlert(
                    "Whoops...",
                    "Unable to load the audio. Did you choose a valid file? (must be MP3, WAV, M4A or AAC)"
                    );
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task PickImage()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    IsBusy = true;

                    using var stream = await result.OpenReadAsync();

                    var uploadedImage = await _dataService.UploadFile(stream, result.FileName, "images");
                    NewImagePath = uploadedImage.Path;
                    NewImageUrl = uploadedImage.PublicUrl;
                }
            }
            catch
            {
                await _dialogService.ShowAlert(
                    "Whoops...",
                    "Unable to load the picture. Did you choose a valid file (must be an image file)"
                    );
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Saves the new audio in the audio table (so it also shows up in the library),
        // then ticks it in the list so it ends up in the playlist.
        [RelayCommand]
        private async Task UploadAudio()
        {
            if (IsBusy) return;

            var userId = _authService.CurrentUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                await _dialogService.ShowAlert(
                    "Whoops...",
                    "Sorry, you need to have an account to upload audios. It's free to sign up!"
                    );
                return;
            }

            if (string.IsNullOrWhiteSpace(NewAudioName) || string.IsNullOrWhiteSpace(NewAudioUrl))
            {
                await _dialogService.ShowAlert("Whoops...", "Remember to fill all required fields");
                return;
            }

            try
            {
                IsBusy = true;

                var newAudioId = await _dataService.AddAudio(new AudioInsert
                {
                    AudioName = NewAudioName,

                    ImageUrl = NewImageUrl,
                    ImagePath = NewImagePath,

                    AudioUrl = NewAudioUrl,
                    AudioPath = NewAudioPath,

                    AdminId = userId
                });

                ResetUploadForm();

                // RefreshMyAudios handles its own errors, so a failed refresh can never
                // end up in the catch below and delete files of an audio that IS saved.
                await RefreshMyAudios(alsoSelectId: newAudioId);
            }
            catch
            {
                // Files are in storage, but the audio row was not saved -> remove the files
                await DeleteUploadedFiles();

                await _dialogService.ShowAlert("Error", "Something didn't work, try again");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteUploadedFiles()
        {
            try
            {
                // DeleteFileFromStorage simply returns when the path is empty
                await _dataService.DeleteFileFromStorage("audio-files", NewAudioPath);
                await _dataService.DeleteFileFromStorage("images", NewImagePath);
            }
            catch
            {
                // best effort - nothing more to do if this fails too
            }
        }

        private void ResetUploadForm()
        {
            NewAudioName = string.Empty;
            NewImageUrl = string.Empty;
            NewImagePath = string.Empty;
            NewAudioUrl = string.Empty;
            NewAudioPath = string.Empty;
            ShowUploadForm = false;
        }




        

        // ------- Add the playlist -------

        [RelayCommand]
        private async Task AddPlaylist()
        {
            if (IsBusy) return;

            var userId = _authService.CurrentUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                await _dialogService.ShowAlert(
                    "Sorry...",
                    "You need an account to make a playlist. Sign up, it's free!"
                    );
                return;
            }

            if (string.IsNullOrWhiteSpace(NewPlaylistName))
            {
                await _dialogService.ShowAlert(
                    "Whoops...",
                    "Remember to name your playlist"
                    );
                return;
            }

            long playlistId = 0;

            try
            {
                IsBusy = true;

                // 1) Insert the playlist first - we need its id for the junction table
                playlistId = await _dataService.AddPlaylist(new PlaylistInsert
                {
                    PlaylistName = NewPlaylistName.Trim(),
                    AdminId = userId
                });

                /* The above might be the same like the following:
                var newPlaylist = new PlaylistInsert
                {
                    PlaylistName = NewPlaylistName,
                    AdminId = userId ?? string.Empty
                };

                await _dataService.AddPlaylist(newPlaylist);
                 */

                // 2) One junction row per ticked audio, sent as ONE insert (all or nothing)
                var playlistAudios = MyAudios
                    .Where(a => a.IsSelected)
                    .Select(a => new PlaylistAudioInsert
                    {
                        PlaylistId = playlistId,
                        AudioId = a.Audio.Id
                    })
                    .ToList();

                if (playlistAudios.Count > 0)
                {
                    await _dataService.AddAudiosToPlaylist(playlistAudios);
                }
            }
            catch
            {
                // The playlist row may exist without its audios -> remove it again
                if (playlistId != 0)
                {
                    try
                    {
                        await _dataService.DeletePlaylist(playlistId);
                    }
                    catch
                    {
                        // best effort
                    }
                }

                await _dialogService.ShowAlert("Error", "Something didn't work, try again");
                return;
            }
            finally
            {
                IsBusy = false;
            }

            try
            {
                // Navigate back to the previous page (PlaylistViewModel) just like pressing the back button
                await _navigationService.GoBackAsync();
            }
            catch
            {
                await _dialogService.ShowAlert("An error occured, but playlist is saved...", "Go back to see it in your playlists");
            }
        }
    }
}
