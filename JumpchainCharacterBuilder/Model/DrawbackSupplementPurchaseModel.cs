using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents an individual selection from the Drawback Supplement.
    /// </summary>
    public partial class DrawbackSupplementPurchase : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the Drawback.
        /// </summary>
        [ObservableProperty]
        private string _name = "";

        /// <summary>
        /// Represents the category a Drawback should be placed in.
        /// </summary>
        [ObservableProperty]
        private string _category = "";

        /// <summary>
        /// Represents the description of the Drawback.
        /// </summary>
        [ObservableProperty]
        private string _description = "";

        /// <summary>
        /// Represents the number of Choice Points given by the Drawback.
        /// </summary>
        [ObservableProperty]
        private int _valueChoicePoints = 0;

        /// <summary>
        /// Represents the number of Choice Points given by the Drawback
        /// that can only be spent on Items.
        /// </summary>
        [ObservableProperty]
        private int _valueItemPoints = 0;

        /// <summary>
        /// Represents the number of Choice Points that the Drawback
        /// gives to the Jumper's Companions.
        /// </summary>
        [ObservableProperty]
        private int _valueCompanionPoints = 0;

        /// <summary>
        /// Represents the number of Warehouse Points given by the
        /// Drawback.
        /// </summary>
        [ObservableProperty]
        private int _valueWarehousePoints = 0;

        /// <summary>
        /// Represents which Jumps this Drawback was suspended for.
        /// </summary>
        [ObservableProperty]
        private List<bool> _suspend = [];

        /// <summary>
        /// Represents which Jumps this Drawback was suspended during.
        /// </summary>
        [ObservableProperty]
        private List<DrawbackSuspendModel> _suspendList = [];

        /// <summary>
        /// Represents the Jump number that this Drawback was revoked
        /// on. A 0 stands for a Drawback that has not been revoked.
        /// </summary>
        [ObservableProperty]
        private int _revoke = 0;

        /// <summary>
        /// Represents whether this Drawback gives points during Gauntlets.
        /// This only works if the relevant setting is applied to the Supplement in the Options section.
        /// </summary>
        [ObservableProperty]
        private bool _applyGauntlet = false;

        public DrawbackSupplementPurchase()
        {

        }

        public DrawbackSupplementPurchase(string name, int jumpCount)
        {
            Name = name;
            for (int i = 0; i < jumpCount; i++)
            {
                Suspend.Add(false);
            }
        }
    }
}
