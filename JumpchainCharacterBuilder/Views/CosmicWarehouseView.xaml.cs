using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for CosmicWarehouseView.xaml
    /// </summary>
    public partial class CosmicWarehouseView : UserControl
    {
        public CosmicWarehouseView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<CosmicWarehouseViewModel>();
            }
        }

        public CosmicWarehouseViewModel CosmicWarehouseViewModel => (CosmicWarehouseViewModel)DataContext;

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
