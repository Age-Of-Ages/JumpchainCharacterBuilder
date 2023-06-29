using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Linq;
using System.Windows;
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
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<TopMenuViewModel>();
            }
        }

        public TopMenuViewModel TopMenuViewModel => (TopMenuViewModel)DataContext;

        private void InputFormatter_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<InputFormatterView>().Any())
            {
                InputFormatterView inputFormatter = new();

                inputFormatter.Show();
                inputFormatter.Owner = Application.Current.MainWindow;
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsView settingsView = new();

            settingsView.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutView aboutView = new();

            aboutView.ShowDialog();
        }
    }
}
