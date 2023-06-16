using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for CosmicPassportView.xaml
    /// </summary>
    public partial class CosmicPassportView : UserControl
    {
        public CosmicPassportView()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService<CosmicPassportViewModel>();

        }

        CosmicPassportViewModel CosmicPassportViewModel => (CosmicPassportViewModel)DataContext;
    }
}
