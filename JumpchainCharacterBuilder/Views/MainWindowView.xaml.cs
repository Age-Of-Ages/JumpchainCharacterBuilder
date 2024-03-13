using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Windows;

namespace JumpchainCharacterBuilder
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<MainWindowViewModel>();

                string currentTheme = CfgAccess.ReadSingleSetting("Theme");

                if (currentTheme == "Error")
                {
                    currentTheme = "Light";
                }

                ResourceDictionary dictionary = App.Current.Resources;
                dictionary.MergedDictionaries.Clear();
                dictionary.MergedDictionaries.Add(new() { Source = new("/PresentationFramework.Aero2;component/themes/Aero2.NormalColor.xaml", UriKind.RelativeOrAbsolute) });
                dictionary.MergedDictionaries.Add(new() { Source = new("Styles.xaml", UriKind.RelativeOrAbsolute) });

                if (currentTheme == "Light")
                {
                    dictionary.MergedDictionaries.Add(new() { Source = new("Themes/ThemeLight.xaml", UriKind.RelativeOrAbsolute) });
                }
                else if (currentTheme == "Dark")
                {
                    dictionary.MergedDictionaries.Add(new() { Source = new("Themes/ThemeDark.xaml", UriKind.RelativeOrAbsolute) });
                }
            }
        }

        public MainWindowViewModel MainWindowViewModel => (MainWindowViewModel)DataContext;

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var viewModel = (MainWindowViewModel)DataContext;

            viewModel.TriggerSaveCommand.Execute(this);

            bool saveSucceeded = viewModel.SaveSucceeded;

            if (!saveSucceeded)
            {
                e.Cancel = true;
            }
        }
    }
}
