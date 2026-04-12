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

        // In Shaa Allah, ObservableCollection is used to update the ui if a change happens
        // (gives notification when items get added or removed). 
        public ObservableCollection<Audio> Audios { get; set; } = new ObservableCollection<Audio>();

        public LibraryViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }




        [RelayCommand]
        public async Task GetAudio()
        {
            Audios.Clear(); // (auto generated comment) Clear the existing collection to avoid duplicates

            try
            {
                var audios = await _dataService.GetAudios();
                if (audios.Any())
                {
                    foreach (var audio in audios)
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
        // ** TODO: In Shaa Allah, add a xaml file that's used to add audios **
        [RelayCommand]
        private async Task AddAudio() => await Shell.Current.GoToAsync("AddAudioPage");




        [RelayCommand]
        private async Task DeleteAudio(Audio audio)
        {
            var result = await Shell.Current.DisplayAlertAsync("Delete", $"Are you sure you want to remove from the list: \"{audio.AudioName}\"?", "Yes", "No");

            if (result is true)
            {
                try
                {
                    await _dataService.DeleteAudio(audio.Id);
                    await GetAudio(); // In Shaa Allah, refresh the list after a deletion
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
                }
            }

        }
    }
}

