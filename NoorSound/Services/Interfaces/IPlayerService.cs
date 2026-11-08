using CommunityToolkit.Maui.Views;
using NoorSound.Models;

namespace NoorSound.Services.Interfaces
{
    public interface IPlayerService
    {
        // -- State --
        Audio? CurrentAudio { get; }
        bool IsPlaying { get; }
        TimeSpan Position { get; }
        TimeSpan Duration { get; }

        // Raised when anything changes (audio, play/pause, position, duration).
        // It can be raised from a background thread.
        event EventHandler? Changed;

        // Raised when an audio can't be played (for example a bad link or no internet).
        event EventHandler<string>? PlaybackFailed;


        // -- Actions --

        // Gives the player the ONE MediaElement of the app
        void Attach(MediaElement element);

        // Starts the audio. If it is already the current audio, it is not restarted.
        Task PlayAsync(Audio audio);

        Task TogglePlayPauseAsync();

        Task SeekToAsync(TimeSpan position);
    }
}
