using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for TopMenuUserControl.xaml
    /// </summary>
    public partial class TopMenuView : UserControl
    {
        public TopMenuView()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                // Design-mode specific functionality
            }
            else
            {
                this.DataContext = App.Current.Services.GetService<TopMenuViewModel>();
            }

        }

        public TopMenuViewModel TopMenuViewModel => (TopMenuViewModel)DataContext;
    }
}
