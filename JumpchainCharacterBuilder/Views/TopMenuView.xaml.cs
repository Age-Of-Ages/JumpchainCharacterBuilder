using JumpchainCharacterBuilder.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JumpchainCharacterBuilder.Views
{
    /// <summary>
    /// Interaction logic for TopMenuUserControl.xaml
    /// </summary>
    public partial class TopMenuView : UserControl
    {
        public TopMenuView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = App.Current.Services.GetService<TopMenuViewModel>();

                ThemeChange(TopMenuViewModel.Theme);
            }
        }

        public TopMenuViewModel TopMenuViewModel => (TopMenuViewModel)DataContext;

        private void InputFormatter_Click(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<InputFormatterView>().Any())
            {
                InputFormatterView inputFormatter = new()
                {
                    Owner = Application.Current.MainWindow
                };

                inputFormatter.Show();
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsView settingsView = new()
            {
                Owner = Application.Current.MainWindow
            };

            settingsView.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutView aboutView = new()
            {
                Owner = Application.Current.MainWindow
            };

            aboutView.ShowDialog();
        }

        private void JumpRandomizer_Click(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<JumpRandomizerSelectorView>().Any())
            {
                JumpRandomizerSelectorView jumpRandomizer = new()
                {
                    Owner = Application.Current.MainWindow
                };

                jumpRandomizer.Show();
            }
        }

        private void JumpRandomizerSettings_Click(object sender, RoutedEventArgs e)
        {
            JumpRandomizerListView randomizerSettingsView = new()
            {
                Owner = Application.Current.MainWindow
            };

            randomizerSettingsView.ShowDialog();
        }

        private static void ThemeChange(string currentTheme)
        {
            ResourceDictionary dictionary = App.Current.Resources;
            //dictionary.MergedDictionaries.Clear();
            //dictionary.MergedDictionaries.Add(new() { Source = new("/PresentationFramework.Aero2;component/themes/Aero2.NormalColor.xaml", UriKind.RelativeOrAbsolute) });
            //dictionary.MergedDictionaries.Add(new() { Source = new("Styles.xaml", UriKind.RelativeOrAbsolute) });

            dictionary.MergedDictionaries.RemoveAt(2);

            if (currentTheme == "Light")
            {
                dictionary.MergedDictionaries.Add(new() { Source = new("Themes/ThemeLight.xaml", UriKind.RelativeOrAbsolute) });
            }
            else if (currentTheme == "Dark")
            {
                dictionary.MergedDictionaries.Add(new() { Source = new("Themes/ThemeDark.xaml", UriKind.RelativeOrAbsolute) });
            }
        }

        private void ThemeChangeLight_Click(object sender, RoutedEventArgs e)
        {
            ThemeChange("Light");
        }

        private void ThemeChangeDark_Click(object sender, RoutedEventArgs e)
        {
            ThemeChange("Dark");
        }
    }
}
