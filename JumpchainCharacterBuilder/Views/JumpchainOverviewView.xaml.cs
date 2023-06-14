using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for JumpchainOverviewView.xaml
    /// </summary>
    public partial class JumpchainOverviewView : UserControl
    {
        public JumpchainOverviewView()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                // Design-mode specific functionality
            }
            else
            {
                this.DataContext = App.Current.Services.GetService<JumpchainOverviewViewModel>();
            }
        }

        public JumpchainOverviewViewModel JumpchainOverviewViewModel => (JumpchainOverviewViewModel)DataContext;
    }
}
