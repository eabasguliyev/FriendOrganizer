using System.Windows;

namespace FriendOrganizer.UI.Views.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel, MessageBoxImage.Information);

            return result == MessageBoxResult.OK ? MessageDialogResult.Ok : MessageDialogResult.Cancel;
        }

        public void ShowInfoDialog(string text)
        {
            MessageBox.Show(text, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public enum MessageDialogResult
    {
        Ok,
        Cancel
    }
}