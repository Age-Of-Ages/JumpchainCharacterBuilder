using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder.Model
{
    public partial class JumpRandomizerList : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the list.
        /// </summary>
        [ObservableProperty]
        private string _listName = "";

        /// <summary>
        /// Represents the Jump entries contained within the list.
        /// </summary>
        [ObservableProperty]
        private List<JumpRandomizerEntry> _listEntries = new();
    }
}
