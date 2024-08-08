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
    /// Interaction logic for CosmicLockerView.xaml
    /// </summary>
    public partial class CosmicLockerView : UserControl
    {
        public CosmicLockerView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<CosmicLockerViewModel>();
            }
        }

        public CosmicLockerViewModel CosmicLockerViewModel => (CosmicLockerViewModel)DataContext;

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
