using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace JumpchainCharacterBuilder.ViewModel
{
    [ObservableRecipient]
    public partial class ViewModelBase : ObservableValidator, INotifyPropertyChanged
    {

    }
}
