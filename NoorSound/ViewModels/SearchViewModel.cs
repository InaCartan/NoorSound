using CommunityToolkit.Mvvm.ComponentModel;
using NoorSound.Models;
using NoorSound.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace NoorSound.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        public ObservableCollection<Audio> Audios {get; set;} = new ObservableCollection<Audio>();

        public SearchViewModel()
        {
            
        }

        
    }
}