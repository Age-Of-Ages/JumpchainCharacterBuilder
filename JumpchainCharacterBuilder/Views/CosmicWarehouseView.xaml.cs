using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for CosmicWarehouseView.xaml
    /// </summary>
    public partial class CosmicWarehouseView : UserControl
    {
        public CosmicWarehouseView()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                // Design-mode specific functionality
            }
            else
            {
                this.DataContext = App.Current.Services.GetService<CosmicWarehouseViewModel>();
            }
        }

        public CosmicWarehouseViewModel CosmicWarehouseViewModel => (CosmicWarehouseViewModel)DataContext;
    }
}
