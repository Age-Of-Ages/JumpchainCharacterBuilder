using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    public partial class PurchaseCategoryStatistics : ObservableValidator
    {
        [ObservableProperty]
        private string _name = "";

        [ObservableProperty]
        private int _total = 0;
    }
}
