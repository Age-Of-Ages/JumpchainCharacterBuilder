using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for JumpchainOptionsView.xaml
    /// </summary>
    public partial class JumpchainOptionsView : UserControl
    {
        public JumpchainOptionsView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<JumpchainOptionsViewModel>();
            }
        }

        public JumpchainOptionsViewModel JumpchainOptionsViewModel => (JumpchainOptionsViewModel)DataContext;
    }
}
