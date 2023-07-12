using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
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
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<InputFormatterViewModel>();
            }
        }

        public InputFormatterViewModel InputFormatterViewModel => (InputFormatterViewModel)DataContext;
    }
}
