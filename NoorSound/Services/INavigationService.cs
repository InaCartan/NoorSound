using System;
using System.Collections.Generic;
using System.Text;

namespace NoorSound.Services
{
    public interface INavigationService
    {
        Task GoToAsync(string route);
        Task GoBackAsync();
    }
}
