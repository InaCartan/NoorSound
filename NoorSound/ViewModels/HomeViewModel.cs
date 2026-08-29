// using NoorSound.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using NoorSound.Services;
using NoorSound.Services.Interfaces;
using System.Collections.ObjectModel;
namespace NoorSound.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        // In Shaa Allah, ObservableCollection is used to update the ui if a change happens
        // (gives notification when items get added or removed). 
        public ObservableCollection<Audio> Audios { get; set; } = new ObservableCollection<Audio>();

        public HomeViewModel(IDataService dataService, IDialogService dialogService, INavigationService navigationService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _navigationService = navigationService;

        }

        [RelayCommand]
        public async Task LoadAudios()
        {
            try
            {
                Audios.Clear();

                var audios = await _dataService.GetAudios();

                foreach (var audio in audios)
                {
                    Audios.Add(audio);
                }
            }
            catch
            {
                await _dialogService.ShowAlert("Error", "Unable to load audios, try to refresh page");
            }
        }

        // In Shaa Allah ta'ala, this func navigates to AudioPlayerPage
        [RelayCommand]
        private async Task NavAudioPlayerPage(Audio audio)
        {
            await _navigationService.GoToAsyncWithObject(
               AppRoutes.AudioPlayer,
               new Dictionary<string, object> { { "Audio", audio } });
        }
    }
}

