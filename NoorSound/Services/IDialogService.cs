

namespace NoorSound.Services
{
    public interface IDialogService
    {
        Task ShowAlert(string title, string message, string cancel = "OK");

        Task<bool> ShowConfirmation(
            string title,
            string message,
            string accept = "Yes",
            string cancel = "No"
            );
    }
}
