// using NoorSound.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using NoorSound.Services;
using System.CodeDom;
using System.Collections.ObjectModel;
namespace NoorSound.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IDataService _dataService;

        // In Shaa Allah, ObservableCollection is used to update the ui if a change happens
        // (gives notification when items get added or removed). 
        public ObservableCollection<Audio> Audios { get; set; } = new ObservableCollection<Audio>();

        public HomeViewModel(IDataService dataService)
        {
            _dataService = dataService;
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
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        }

    }
}

