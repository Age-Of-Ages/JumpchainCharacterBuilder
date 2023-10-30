namespace JumpchainCharacterBuilder.Interfaces
{
    public interface IDialogService
    {
        bool ConfirmDialog(string message);

        void NotificationDialog(string message);
    }
}
