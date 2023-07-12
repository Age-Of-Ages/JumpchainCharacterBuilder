using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

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
