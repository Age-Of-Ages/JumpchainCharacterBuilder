using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for DrawbackSupplementView.xaml
    /// </summary>
    public partial class DrawbackSupplementView : UserControl
    {
        public DrawbackSupplementView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<DrawbackSupplementViewModel>(); 
            }
        }

        public DrawbackSupplementViewModel DrawbackSupplementViewModel => (DrawbackSupplementViewModel)DataContext;
    }
}
