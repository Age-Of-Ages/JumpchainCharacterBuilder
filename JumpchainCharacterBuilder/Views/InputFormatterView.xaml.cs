using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for InputFormattingView.xaml
    /// </summary>
    public partial class InputFormatterView : Window
    {
        public InputFormatterView()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService<InputFormatterViewModel>();
        }

        public InputFormatterViewModel InputFormatterViewModel => (InputFormatterViewModel)DataContext;
    }
}
