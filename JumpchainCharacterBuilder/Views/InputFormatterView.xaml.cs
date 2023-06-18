using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

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
