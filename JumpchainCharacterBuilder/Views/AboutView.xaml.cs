using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<AboutViewModel>();
            }
        }

        public AboutViewModel AboutViewModel => (AboutViewModel)DataContext;

        private void Github_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true
            });
            e.Handled = true;
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
