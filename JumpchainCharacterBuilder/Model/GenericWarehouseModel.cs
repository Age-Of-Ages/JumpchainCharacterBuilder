using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    public partial class GenericWarehouseModel : WarehouseUniversal
    {
        /// <summary>
        /// Represents whether the generic Warehouse Supplement allows
        /// purchases or not.
        /// If it is a single block of text then this should probably be
        /// left as false.
        /// </summary>
        [ObservableProperty]
        private bool _purchasesAllowed = false;

        /// <summary>
        /// Represents the full description of this Warehouse Supplement.
        /// If this Supplement is just a single block of text then that
        /// information will be stored here.
        /// </summary>
        [ObservableProperty]
        private string _fullDescription = "";
    }
}
