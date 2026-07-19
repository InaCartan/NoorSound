using System;
using System.Collections.Generic;
using System.Text;

namespace NoorSound.Services
{
    public interface IStartupService
    {
        Task InitializeAsync();
    }
}
