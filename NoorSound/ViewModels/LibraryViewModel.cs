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
        public ObservableCollection<Audio> Audios { get; set; } = new();


        public LibraryViewModel(IDataService dataService)
        {
            _dataService = dataService;

            //// Listen for navigation events to refresh the audio list
            //Shell.Current.Navigated += (sender, args) =>
            //{
            //    if (args.Source == ShellNavigationSource.Pop && args.Current?.Location.OriginalString.Contains("LibraryPage") == true)
            //    {
            //        MainThread.BeginInvokeOnMainThread(async () => await GetAudio());
            //    }
            //};
        }


        [RelayCommand]
        public async Task GetAudio()
        {
            Audios.Clear(); // (almost auto generated comment) In Shaa Allah, ".Clear" clear the existing collection thus avoiding duplicates
            // Audios [ 1,2,3,4,5]

            try
            {
                var audios = await _dataService.GetAudios();
                if (audios.Any())
                {
                    foreach (var audio in audios)
                    {
                        Audios.Add(audio); // [1,2,3,4,5,6,7,8,1,10,11,2]
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

