using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            App.ListViewResizeEvent(sender);
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            App.ListViewResizeEvent(sender);
        }

        private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            App.ListViewResizeEvent(sender);
        }
    }
}
