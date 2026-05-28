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
        private readonly LibraryViewModel _libraryViewModel;

        [ObservableProperty]
        private string newAudioName;

        [ObservableProperty] 
        private Admin newAdmin;

        [ObservableProperty]
        private string newImageUrl;

        [ObservableProperty]
        private string newAudioUrl;

        [ObservableProperty]
        private bool isBusy;

        public AddAudioViewModel(IDataService dataService, LibraryViewModel libraryViewModel)
        {
            _dataService = dataService;
            _libraryViewModel = libraryViewModel;
            // (almost auto generated) In Shaa Allah ta'la, initialize the NewAdmin property to avoid null reference issues
            NewAdmin = new Admin(); 
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

                    NewAudioUrl = await _dataService.UploadFile(stream, result.FileName, "audio-files"); 
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
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

                    NewImageUrl = await _dataService.UploadFile(stream, result.FileName, "images");
                }
            }    
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
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
            try
            {
                if (!string.IsNullOrEmpty(NewAudioName) && !string.IsNullOrEmpty(NewAudioUrl) && !string.IsNullOrEmpty(NewAdmin.Name))
                {

                    AudioInsert audio = new AudioInsert()
                    {
                        AudioName = NewAudioName,
                        ImageUrl = NewImageUrl,
                        AudioUrl = NewAudioUrl,
                        AdminId = _authService.CurrentUserId()
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
