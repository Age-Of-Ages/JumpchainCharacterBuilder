using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
