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
            this.DataContext = App.Current.Services.GetService<TopMenuViewModel>();

        }

        public TopMenuViewModel TopMenuViewModel => (TopMenuViewModel)DataContext;

        private void InputFormatter_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<InputFormatterView>().Any())
            {
                InputFormatterView inputFormatter = new();

                inputFormatter.Show();
            }
        }
    }
}
