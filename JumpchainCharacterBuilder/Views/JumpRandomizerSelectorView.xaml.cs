using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for JumpRandomizerSelectorView.xaml
    /// </summary>
    public partial class JumpRandomizerSelectorView : Window
    {
        public JumpRandomizerSelectorView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<JumpRandomizerSelectorViewModel>();
            }
        }

        public JumpRandomizerSelectorViewModel JumpRandomizerSelectorViewModel => (JumpRandomizerSelectorViewModel)DataContext;

        private void JumpDocument_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (MessageBox.Show($"Open the link {e.Uri.AbsoluteUri} in your default browser?", 
                                "Confirm action", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
                {
                    UseShellExecute = true
                });
            }
            
            e.Handled = true;
        }
    }
}
