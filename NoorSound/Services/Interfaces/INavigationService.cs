using System;
using System.Collections.Generic;
using System.Text;

namespace NoorSound.Services.Interfaces
{
    public interface INavigationService
    {
        Task GoToAsyncWithObject(string route, IDictionary<string, object> parameters);
        Task GoToAsync(string route);
        Task GoBackAsync();
    }
}
