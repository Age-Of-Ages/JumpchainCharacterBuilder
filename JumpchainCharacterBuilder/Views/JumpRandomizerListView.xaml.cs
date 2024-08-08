using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            if (sender is ListView listView && listView.Tag != null)
            {
                if (listView.Tag is string indexString)
                {
                    List<string> indexStringList = [.. indexString.Split(',')];
                    List<int> columnIndexes = [];

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
                    List<string> indexStringList = [.. indexString.Split(',')];
                    List<int> columnIndexes = [];

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
                    List<string> indexStringList = [.. indexString.Split(',')];
                    List<int> columnIndexes = [];

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
