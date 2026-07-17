using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Services;
using NoorSound.Models;

namespace NoorSound.ViewModels
{
    public partial class AddAudioViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly IAuthService _authService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;


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

        [ObservableProperty]
        public partial bool IsBusy { get; set; }

        public AddAudioViewModel(
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

                    // In Shaa Allah, IsBusy = true, UI shows loading icon - IsBusy = false, loading icon dissaperes.
                    // In Shaa Allah, the variable is used in a XAML file
                    IsBusy = true;

                    // ** (almost) Auto generated comment **
                    // In Shaa Allah, this will open the selected audio file as a stream,
                    // that can be uploaded to Supabase using the UploadFile method
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

        // Why private?
        // ** This response is (almost) generated - Don't know if it's correct **
        // Because we don't want this method to be accessible from outside the class.
        // It's only meant to be called by the UI when the user clicks a button or something like that.
        // By making it private, we can ensure that it can only be called from within the class, In Shaa Allah the code will then be organized and maintainable.
        [RelayCommand]
        private async Task AddAudio()
        {
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
                AudioInsert audio = new AudioInsert()
                {
                    AudioName = NewAudioName,

                    ImageUrl = NewImageUrl,
                    ImagePath = NewImagePath,

                    AudioUrl = NewAudioUrl,
                    AudioPath = NewAudioPath,

                    AdminId = userId
                };

                await _dataService.AddAudio(audio);

            }
            catch
            {
                if (!string.IsNullOrWhiteSpace(NewAudioPath))
                {
                    await _dataService.DeleteFileFromStorage("audio-files", NewAudioPath);
                }

                if (!string.IsNullOrWhiteSpace(NewImagePath))
                {
                    await _dataService.DeleteFileFromStorage("images", NewImagePath);
                }

                await _dialogService.ShowAlert("Hmm...", "Something didn't work, try again");
                return;
            }

            try
            {
                // ** (the following comment is "almost" generated in VS 2022 - maybe it's right - ) **
                // Navigate back to the previous page (LibraryViewModel) just like pressing the back button 
                await _navigationService.GoBackAsync();
            }
            catch
            {
                await _dialogService.ShowAlert("An error occured, but audio is saved...", "Go back to see it in library");
            }


        }

    }
}
