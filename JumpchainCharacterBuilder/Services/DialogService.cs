using System.Windows;

namespace JumpchainCharacterBuilder.Services
{

    public interface IDialogService
    {
        bool ConfirmDialog(string message);
    }

    public class DialogService : IDialogService
    {
        public bool ConfirmDialog(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Confirm action", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes ? true : false;
        }
    }
}
