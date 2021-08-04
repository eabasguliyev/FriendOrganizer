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
    }

    public enum MessageDialogResult
    {
        Ok,
        Cancel
    }
}