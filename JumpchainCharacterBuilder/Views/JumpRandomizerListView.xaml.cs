using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;

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
