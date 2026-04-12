using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Services;
using NoorSound.Models;

namespace NoorSound.ViewModels
{
    public partial class AddAudioViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly LibraryViewModel _libraryViewModel;

        [ObservableProperty]
        private string audioName;

        [ObservableProperty]
        private string adminName;

        [ObservableProperty]
        private string imageUrl;

        [ObservableProperty]
        private string audioUrl;


        public AddAudioViewModel(IDataService dataService, LibraryViewModel libraryViewModel)
        {
            _dataService = dataService;
            _libraryViewModel = libraryViewModel;
        }

        // Why private?
        // ** This response is (almost) generated - Don't know if it's correct **
        // Because we don't want this method to be accessible from outside the class.
        // It's only meant to be called by the UI when the user clicks a button or something like that.
        // By making it private, we can ensure that it can only be called from within the class, In Shaa Allah the code will then be organized and maintainable.
        [RelayCommand]
        private async Task AddAudio()
        {
            try
            {
                if (!string.IsNullOrEmpty(AudioName) && !string.IsNullOrEmpty(AudioUrl))
                {

                    Audio audio = new Audio()
                    {
                        AudioName = AudioName,
                        ImageUrl = ImageUrl,
                        AudioUrl = AudioUrl
                    };

                    await _dataService.AddAudio(audio);

                    // In Shaa Allah, by getting the data from the db (using GetAudio), it will refresh the
                    // list of audios shown in the home view UI, after adding a new audio.
                    await _libraryViewModel.GetAudio();

                    // ** (the following comment is "almost" generated in VS 2022 - maybe it's right - ) **
                    // Navigate back to the previous page (LibraryViewModel) just like pressing the back button 
                    await Shell.Current.GoToAsync(".."); 
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Remember to fill all required fields", "OK");
                }
                
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }


        }

    }
}
