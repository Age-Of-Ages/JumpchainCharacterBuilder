using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService<SettingsViewModel>();
        }

        public SettingsViewModel SettingsViewModel => (SettingsViewModel)DataContext;
    }
}
