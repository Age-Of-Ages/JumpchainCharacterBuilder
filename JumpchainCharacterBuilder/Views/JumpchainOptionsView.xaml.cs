using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for JumpchainOptionsView.xaml
    /// </summary>
    public partial class JumpchainOptionsView : UserControl
    {
        public JumpchainOptionsView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<JumpchainOptionsViewModel>();
            }
        }

        public JumpchainOptionsViewModel JumpchainOptionsViewModel => (JumpchainOptionsViewModel)DataContext;

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

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 1300)
            {
                VisualStateManager.GoToState(this, "CombinedTabState", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "SplitTabState", false);
            }
        }
    }
}
