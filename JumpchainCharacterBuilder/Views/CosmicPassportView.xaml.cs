using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for CosmicPassportView.xaml
    /// </summary>
    public partial class CosmicPassportView : UserControl
    {
        public CosmicPassportView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<CosmicPassportViewModel>();

            }
        }

        public CosmicPassportViewModel CosmicPassportViewModel => (CosmicPassportViewModel)DataContext;

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
