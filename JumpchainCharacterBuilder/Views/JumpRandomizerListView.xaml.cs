using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for JumpRandomizerListView.xaml
    /// </summary>
    public partial class JumpRandomizerListView : Window
    {
        public JumpRandomizerListView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<JumpRandomizerListViewModel>();
            }
        }

        public JumpRandomizerListViewModel JumpRandomizerListViewModel => (JumpRandomizerListViewModel)DataContext;

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Save changes?", "Confirm action", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var viewModel = (JumpRandomizerListViewModel)DataContext;

                viewModel.SendChangesCommand.Execute(this);
            }
        }
    }
}
