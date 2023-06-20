using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService<StatisticsViewModel>();
        }

        StatisticsViewModel StatisticsViewModel => (StatisticsViewModel)DataContext;
    }
}
