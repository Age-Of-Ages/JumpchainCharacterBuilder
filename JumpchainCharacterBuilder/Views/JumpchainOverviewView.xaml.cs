using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for JumpchainOverviewView.xaml
    /// </summary>
    public partial class JumpchainOverviewView : UserControl
    {
        public JumpchainOverviewView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<JumpchainOverviewViewModel>();
            }
        }

        public JumpchainOverviewViewModel JumpchainOverviewViewModel => (JumpchainOverviewViewModel)DataContext;

        private void JumpList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView? listView = sender as ListView;

            listView?.ScrollIntoView(listView.SelectedItem);
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
