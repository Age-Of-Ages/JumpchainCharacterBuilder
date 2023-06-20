using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
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
            this.DataContext = App.Current.Services.GetService<CosmicLockerViewModel>();
        }

        public CosmicLockerViewModel CosmicLockerViewModel => (CosmicLockerViewModel)DataContext;
    }
}
