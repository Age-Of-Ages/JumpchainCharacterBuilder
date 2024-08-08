using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Services;
using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

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

            DispatcherUnhandledException += DispatcherUnhandledExceptionLogging;
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
        private static ServiceProvider ConfigureServices()
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
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddTransient<JumpRandomizerSelectorViewModel>();
            services.AddTransient<JumpRandomizerListViewModel>();

            return services.BuildServiceProvider();
        }

        private void DispatcherUnhandledExceptionLogging(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Exception exception = e.Exception;

            List<string> exceptionData = [];

            if (exception.InnerException != null)
            {
                Exception innerException = exception.InnerException;

                string exceptionString = innerException.ToString();

                exceptionString = ReplaceUserDirectoryRegex().Replace(exceptionString, "~LocalAppDir~");

                exceptionData.Add(exceptionString);
            }
            else
            {
                string exceptionString = exception.ToString();

                exceptionString = ReplaceUserDirectoryRegex().Replace(exceptionString, "~LocalAppDir~");

                exceptionData.Add(exceptionString);
            }


            TxtAccess.WriteLog(exceptionData);
        }

        public static void ResizeGridViewColumns(ListView listView, List<int> indexesToStretch)
        {
            if (listView.View is GridView gridView)
            {
                ScrollViewer scrollViewer = GetChildOfType<ScrollViewer>(listView);
                Visibility? scrollbarVisibility = scrollViewer?.ComputedVerticalScrollBarVisibility;

                double availableWidth;

                if (scrollbarVisibility == Visibility.Visible)
                {
                    availableWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth - 10;
                }
                else
                {
                    availableWidth = listView.ActualWidth - 10;
                }

                List<GridViewColumn> autoSizeColumns = [];

                for (int i = 0; i < gridView.Columns.Count; i++)
                {
                    if (indexesToStretch.Contains(i))
                    {
                        autoSizeColumns.Add(gridView.Columns[i]);
                    }
                    else
                    {
                        availableWidth -= gridView.Columns[i].Width;
                    }
                }

                int sizeSplit = autoSizeColumns.Count;

                if (availableWidth < 0)
                {
                    availableWidth = 0;
                }

                foreach (GridViewColumn column in autoSizeColumns)
                {
                    column.Width = availableWidth / sizeSplit;
                }
            }
        }

        public static T GetChildOfType<T>(DependencyObject dependencyObject) 
            where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var target = VisualTreeHelper.GetChild(dependencyObject, i);

                var output = target as T ?? GetChildOfType<T>(target);
                if (output != null)
                {
                    return output;
                }
            }
            return null;
        }

        [GeneratedRegex(@"[a-zA-Z]:\\.+(?=\\JumpchainCharacterBuilder)")]
        private static partial Regex ReplaceUserDirectoryRegex();
    }
}
