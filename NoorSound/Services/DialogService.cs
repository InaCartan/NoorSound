using System;
using System.Collections.Generic;
using System.Text;

namespace NoorSound.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowAlert(string title, string message, string cancel = "OK")
        {
            await Shell.Current.DisplayAlertAsync(
                title,
                message,
                cancel);
        }

        public async Task<bool> ShowConfirmation(string title, string message, string accept = "Yes", string cancel = "No")
        {
            return await Shell.Current.DisplayAlertAsync(
                title,
                message,
                accept,
                cancel);
        }
    }
}
