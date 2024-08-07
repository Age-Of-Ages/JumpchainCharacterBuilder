using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for JumpRandomizerSelectorView.xaml
    /// </summary>
    public partial class JumpRandomizerSelectorView : Window
    {
        public JumpRandomizerSelectorView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<JumpRandomizerSelectorViewModel>();
            }
        }

        public JumpRandomizerSelectorViewModel JumpRandomizerSelectorViewModel => (JumpRandomizerSelectorViewModel)DataContext;

        private void JumpDocument_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (MessageBox.Show($"Open the link {e.Uri.AbsoluteUri} in your default browser?",
                                "Confirm action", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
                {
                    UseShellExecute = true
                });
            }

            e.Handled = true;
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ListView listView && listView.Tag != null)
            {
                if (listView.Tag is string indexString)
                {
                    List<string> indexStringList = indexString.Split(',').ToList();
                    List<int> columnIndexes = new();

                    for (int i = 0; i < indexStringList.Count; i++)
                    {
                        columnIndexes.Add(int.Parse(indexStringList[i]));
                    }

                    App.ResizeGridViewColumns(listView, columnIndexes);
                }
            }
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is ListView listView && listView.Tag != null)
            {
                if (listView.Tag is string indexString)
                {
                    List<string> indexStringList = indexString.Split(',').ToList();
                    List<int> columnIndexes = new();

                    for (int i = 0; i < indexStringList.Count; i++)
                    {
                        columnIndexes.Add(int.Parse(indexStringList[i]));
                    }

                    App.ResizeGridViewColumns(listView, columnIndexes);
                }
            }
        }

        private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender is ListView listView && listView.Tag != null)
            {
                if (listView.Tag is string indexString)
                {
                    List<string> indexStringList = indexString.Split(',').ToList();
                    List<int> columnIndexes = new();

                    for (int i = 0; i < indexStringList.Count; i++)
                    {
                        columnIndexes.Add(int.Parse(indexStringList[i]));
                    }

                    App.ResizeGridViewColumns(listView, columnIndexes);
                }
            }
        }
    }
}
