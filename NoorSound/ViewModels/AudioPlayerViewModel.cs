using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using NoorSound.Services;
using NoorSound.Services.Interfaces;

namespace NoorSound.ViewModels
{
  
    // Used by BOTH the full player (AudioPlayerPage) and the mini player (MiniPlayerView).
    // It must be registered as a SINGLETON in MauiProgram.cs, so both always show the same state.
    public partial class AudioPlayerViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IPlayerService _player;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        // True while the user drags the slider, so the thumb doesn't jump around
        private bool _isSeeking;

        [ObservableProperty]
        public partial Audio? CurrentAudio { get; set; } = null;

        // The mini player is only visible when this is true
        [ObservableProperty]
        public partial bool HasAudio { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PlayPauseLightIcon))]
        [NotifyPropertyChangedFor(nameof(PlayPauseDarkIcon))]
        public partial bool IsPlaying { get; set; }

        // True while the full player is slid up
        [ObservableProperty]
        public partial bool IsPlayerExpanded { get; set; }




        // From 0 to 1
        [ObservableProperty]
        public partial double Progress { get; set; }

        [ObservableProperty]
        public partial string PositionText { get; set; } = "0:00";

        [ObservableProperty]
        public partial string DurationText { get; set; } = "0:00";

        // Light icon: used in the mini player (on a dark background)
        public string PlayPauseLightIcon => IsPlaying ? "ic_pause_light.png" : "ic_play_light.png";

        // Dark icon: used in the full player (on the gold button)
        public string PlayPauseDarkIcon => IsPlaying ? "ic_pause_dark.png" : "ic_play_dark.png";


        public AudioPlayerViewModel(
            IPlayerService player,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            _player = player;
            _dialogService = dialogService;
            _navigationService = navigationService;

            // This ViewModel lives as long as the app, so there is no need to unsubscribe
            _player.Changed += OnPlayerChanged;
            _player.PlaybackFailed += OnPlaybackFailed;
        }


        // ------- Starting an audio (called by Shell when HomeViewModel navigates with the audio) -------

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Audio", out var value) &&
                value is Audio audio)
            {
                // On purpose not awaited (this method can't be async). PlayAudioAsync handles its own errors.
                _ = PlayAudioAsync(audio);
            }
        }

        private async Task PlayAudioAsync(Audio audio)
        {
            try
            {
                await _player.PlayAsync(audio);
            }
            catch
            {
                await _dialogService.ShowAlert("Whoops...", "Unable to play this audio.");
            }
        }


        // ------- Commands -------

        [RelayCommand]
        private async Task TogglePlayPause()
        {
            try
            {
                await _player.TogglePlayPauseAsync();
            }
            catch
            {
                await _dialogService.ShowAlert("Whoops...", "Unable to play this audio.");
            }
        }

        // Tapping (or swiping up) the mini player slides the full player up
        [RelayCommand]
        private void OpenPlayer()
        {
            IsPlayerExpanded = true;
        }

        // Dragging the player down (or the Android back button) closes it
        [RelayCommand]
        private void CollapsePlayer()
        {
            IsPlayerExpanded = false;
        }

        // Called when the user taps an audio on the Home page
        public async Task PlayAndExpandAsync(Audio audio)
        {
            await PlayAudioAsync(audio);   // shows its own alert if it fails

            if (_player.CurrentAudio is not null)
            {
                IsPlayerExpanded = true;
            }
        }


        // ------- Seeking (called by the slider in AudioPlayerPage.xaml.cs) -------

        public void BeginSeek()
        {
            _isSeeking = true;
        }

        public async Task EndSeekAsync(double fraction)
        {
            try
            {
                var duration = _player.Duration;

                if (duration > TimeSpan.Zero)
                {
                    var seconds = Math.Clamp(fraction, 0, 1) * duration.TotalSeconds;
                    await _player.SeekToAsync(TimeSpan.FromSeconds(seconds));
                }
            }
            catch
            {
                // best effort - if seeking fails, the audio just keeps playing from where it was
            }
            finally
            {
                _isSeeking = false;
                Refresh();
            }
        }


        // ------- Keeping the screen in sync with the player -------

        private void OnPlayerChanged(object? sender, EventArgs e)
        {
            // The MediaElement can raise events from another thread, but the UI may only change on the main thread
            if (MainThread.IsMainThread)
            {
                Refresh();
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(Refresh);
            }
        }

        private void OnPlaybackFailed(object? sender, string message)
        {
            _ = MainThread.InvokeOnMainThreadAsync(() =>
                _dialogService.ShowAlert("Whoops...", "Unable to play this audio."));
        }

        private void Refresh()
        {
            CurrentAudio = _player.CurrentAudio;
            HasAudio = CurrentAudio is not null;
            IsPlaying = _player.IsPlaying;

            var position = _player.Position;
            var duration = _player.Duration;

            PositionText = Format(position);
            DurationText = Format(duration);

            if (!_isSeeking)
            {
                Progress = duration.TotalSeconds > 0
                    ? Math.Clamp(position.TotalSeconds / duration.TotalSeconds, 0, 1)
                    : 0;
            }
        }

        private static string Format(TimeSpan time)
        {
            return time.TotalHours >= 1
                ? time.ToString(@"h\:mm\:ss")
                : time.ToString(@"m\:ss");
        }
    }
}
