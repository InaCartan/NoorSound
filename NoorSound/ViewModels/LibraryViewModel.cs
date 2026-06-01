using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Services;
using NoorSound.Models;
using System.Collections.ObjectModel;

namespace NoorSound.ViewModels
{
    public partial class LibraryViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly IAuthService _authService;


        // In Shaa Allah, ObservableCollection is used to update the ui if a change happens
        // (gives notification when items get added or removed). 
        public ObservableCollection<Audio> Audios { get; set; } = new();


        public LibraryViewModel(IDataService dataService, IAuthService authService)
        {
            _dataService = dataService;
            _authService = authService;
        }


        [RelayCommand]
        public async Task LoadAudios()
        {
            try
            {
                Audios.Clear(); // (almost auto generated) In Shaa Allah, clear the existing collection, thus avoiding duplications

                var userId = _authService.CurrentUserId();

                var allAudios = await _dataService.GetAudios();

                var myAudios = allAudios.Where(a => a.AdminId == userId);

                if (myAudios.Any())
                {
                    foreach (var audio in myAudios)
                    {
                        Audios.Add(audio); 
                    }
                }
            }

            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }




        // In Shaa Allah, this func navigates to AddAudioPage.xaml.cs (not the viewmodel)
        [RelayCommand]
        private async Task AddAudio()
        {
            await Shell.Current.GoToAsync("AddAudioPage");
        }



        [RelayCommand]
        private async Task DeleteAudio(Audio audio)
        {
            var result = await Shell.Current.DisplayAlertAsync("Delete", $"Are you sure you want to remove from the playlist: \"{audio.AudioName}\"?", "Yes", "No");

            if (result is true)
            {
                try
                {
                    await _dataService.DeleteAudio(audio.Id);
                    await LoadAudios(); // In Shaa Allah, refresh the list after a deletion
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
                }
            }

        }
    }
}

