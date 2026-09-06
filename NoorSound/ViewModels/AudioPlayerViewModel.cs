using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoorSound.Models;
using NoorSound.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NoorSound.ViewModels
{
    public partial class AudioPlayerViewModel : ObservableObject, IQueryAttributable 
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        public partial Audio? CurrentAudio { get; set; } = null;

        // In Shaa Allah, ObservableCollection is used to update the ui if a change happens
        // (gives notification when items get added or removed). 
        public ObservableCollection<Audio> Audios { get; set; } = new ObservableCollection<Audio>();


        public AudioPlayerViewModel(
            IDataService dataService, 
            IDialogService dialogService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Audio", out var value) &&
                value is Audio audio)
            {
                CurrentAudio = audio;
            }
        }

        // ** Maybe it won't be used, so delete if not used **
        //private async Task LoadAudio(long audioId)
        //{
        //    try
        //    {
        //        var audios = await _dataService.GetAudios();

        //        CurrentAudio = audios.FirstOrDefault(a => a.Id == audioId);
        //    }
        //    catch
        //    {
        //        await _dialogService.ShowAlert(
        //            "Error",
        //            "Unable to load audio");
        //    }
        //}
    }
}
