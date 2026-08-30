using CommunityToolkit.Mvvm.ComponentModel;
using GongSolutions.Wpf.DragDrop;
using System.ComponentModel;

namespace JumpchainCharacterBuilder.ViewModel
{
    [ObservableRecipient]
    public partial class ViewModelBase : ObservableValidator, INotifyPropertyChanged, IDropTarget
    {
        public virtual void DragOver(IDropInfo dropInfo)
        {
            DragDrop.DefaultDropHandler.DragOver(dropInfo);
        }

        public virtual void Drop(IDropInfo dropInfo)
        {
            DragDrop.DefaultDropHandler.Drop(dropInfo);
        }
    }
}
