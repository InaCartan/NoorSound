using System;
using System.Collections.Generic;
using System.Text;

namespace NoorSound.Services
{
    public class NavigationService : INavigationService
    {
        public async Task GoToAsync(string route)
        {
            if (Shell.Current == null)
                return;

            await Shell.Current.GoToAsync(route);
        }

        public async Task GoBackAsync()
        {
            if (Shell.Current == null)
                return;

            await Shell.Current.GoToAsync("..");
        }
    }
}
