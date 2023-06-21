using JumpchainCharacterBuilder.Interfaces;
using System.Windows;

namespace JumpchainCharacterBuilder.Services
{
    public class DialogService : IDialogService
    {
        public bool ConfirmDialog(string message)
        {
            return MessageBox.Show(message, "Confirm action", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }
    }
}
