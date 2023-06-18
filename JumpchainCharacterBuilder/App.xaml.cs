using JumpchainCharacterBuilder.Services;
using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace JumpchainCharacterBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            Services = ConfigureServices();

            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();


            services.AddSingleton<IDialogService, DialogService>();

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<JumpchainOverviewViewModel>();
            services.AddTransient<TopMenuViewModel>();
            services.AddTransient<CosmicPassportViewModel>();
            services.AddTransient<JumpchainOptionsViewModel>();
            services.AddTransient<CosmicWarehouseViewModel>();
            services.AddTransient<DrawbackSupplementViewModel>();
            services.AddTransient<CosmicLockerViewModel>();
            services.AddTransient<ExportViewModel>();
            services.AddTransient<StatisticsViewModel>();
            services.AddTransient<InputFormatterViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
