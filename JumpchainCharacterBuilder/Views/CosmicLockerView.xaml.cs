using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for CosmicLockerView.xaml
    /// </summary>
    public partial class CosmicLockerView : UserControl
    {
        public CosmicLockerView()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                // Design-mode specific functionality
            }
            else
            {
                this.DataContext = App.Current.Services.GetService<CosmicLockerViewModel>();
            }
        }

        public CosmicLockerViewModel CosmicLockerViewModel => (CosmicLockerViewModel)DataContext;
    }
}
