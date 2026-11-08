// ** BismiIllah Ar-Rahmaan Ar-Raheem ** \\

using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using NoorSound.Models;
using NoorSound.Services.Interfaces;

namespace NoorSound.Services
{
    // In Shaa Allah ta'ala, this is the one and only player of the app (a singleton).
    // It controls ONE MediaElement, that exists (hidden) on HomePage, so the audio
    // keeps playing when the user leaves AudioPlayerPage.
    public class PlayerService : IPlayerService
    {
        private MediaElement? _element;

        public Audio? CurrentAudio { get; private set; }

        // Loading counts as playing, so the button shows "pause" right after the user taps an audio
        public bool IsPlaying =>
            _element?.CurrentState is MediaElementState.Playing
                or MediaElementState.Buffering
                or MediaElementState.Opening;

        public TimeSpan Position => _element?.Position ?? TimeSpan.Zero;

        public TimeSpan Duration => _element?.Duration ?? TimeSpan.Zero;

        public event EventHandler? Changed;

        public event EventHandler<string>? PlaybackFailed;


        public void Attach(MediaElement element)
        {
            if (ReferenceEquals(_element, element))
            {
                return;
            }

            // A new element replaces the old one (this only happens if HomePage is created again).
            // The old audio stops, because it belonged to the old element.
            if (_element is not null)
            {
                _element.StateChanged -= OnElementChanged;
                _element.PositionChanged -= OnElementChanged;
                _element.MediaOpened -= OnElementChanged;
                _element.MediaEnded -= OnElementChanged;
                _element.MediaFailed -= OnElementFailed;

                try
                {
                    _element.Stop();
                }
                catch
                {
                    // best effort - the old element may already be gone
                }

                CurrentAudio = null;
            }

            _element = element;

            _element.StateChanged += OnElementChanged;
            _element.PositionChanged += OnElementChanged;
            _element.MediaOpened += OnElementChanged;
            _element.MediaEnded += OnElementChanged;
            _element.MediaFailed += OnElementFailed;

            OnChanged();
        }

        public async Task PlayAsync(Audio audio)
        {
            if (_element is null)
            {
                throw new InvalidOperationException("The player isn't ready yet.");
            }

            // Same audio as before: don't restart it, just continue if it is paused.
            // (A failed audio is loaded again.)
            if (CurrentAudio?.Id == audio.Id && _element.CurrentState != MediaElementState.Failed)
            {
                if (!IsPlaying)
                {
                    await TogglePlayPauseAsync();
                }

                return;
            }

            CurrentAudio = audio;

            _element.Stop();

            // The lock screen and the notification show these
            _element.MetadataTitle = audio.AudioName;
            _element.MetadataArtist = audio.Admin?.Name ?? string.Empty;
            _element.MetadataArtworkUrl = audio.ImageUrl;

            // With ShouldAutoPlay the audio starts by itself as soon as it is loaded
            _element.ShouldAutoPlay = true;
            _element.Source = MediaSource.FromUri(audio.AudioUrl);

            OnChanged();
        }

        public async Task TogglePlayPauseAsync()
        {
            if (_element is null || CurrentAudio is null)
            {
                return;
            }

            if (IsPlaying)
            {
                _element.Pause();
                return;
            }

            // The audio has ended -> start again from the beginning
            if (_element.Duration > TimeSpan.Zero &&
                _element.Position >= _element.Duration - TimeSpan.FromSeconds(1))
            {
                await _element.SeekTo(TimeSpan.Zero);
            }

            _element.Play();
        }

        public async Task SeekToAsync(TimeSpan position)
        {
            if (_element is null || _element.Duration <= TimeSpan.Zero)
            {
                return;
            }

            await _element.SeekTo(position);
        }


        // ------- Events from the MediaElement -------

        private void OnElementChanged(object? sender, EventArgs e)
        {
            OnChanged();
        }

        private void OnElementFailed(object? sender, MediaFailedEventArgs e)
        {
            PlaybackFailed?.Invoke(this, e.ErrorMessage);
            OnChanged();
        }

        private void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
