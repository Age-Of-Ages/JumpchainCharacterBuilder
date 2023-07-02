using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class TopMenuViewModel : ViewModelBase
    {
        #region Fields
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();

        [ObservableProperty]
        private string _saveFileName = "";

        [ObservableProperty]
        private string _theme = "Light";

        [ObservableProperty]
        private bool _lightThemeSelected = true;
        [ObservableProperty]
        private bool _darkThemeSelected = false;

        #endregion

        #region Properties
        partial void OnThemeChanged(string value)
        {
            AppSettings.Theme = value;
            Messenger.Send(new SettingsChangedMessage(true));
            CfgAccess.WriteCfgFile(AppSettings);

            if (value == "Light")
            {
                LightThemeSelected = true;
                DarkThemeSelected = false;
            }
            else if (value == "Dark")
            {
                LightThemeSelected = false;
                DarkThemeSelected = true;
            }
        }

        #endregion

        #region Constructor
        public TopMenuViewModel()
        {

        }

        public TopMenuViewModel(IDialogService dialogService)
        {
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;
            });
            Messenger.Register<SettingsLoadedMessage>(this, (r, m) =>
            {
                AppSettings = m.Value;
            });

            _dialogService = dialogService;
        }

        #endregion

        #region Methods
        private void SavePrompt(bool saveAs)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "Jumper Saves (*.xml)|*.xml",
                DefaultExt = "*.xml",
                AddExtension = true,
                InitialDirectory = Path.Combine(Environment.CurrentDirectory, "Saves"),
                CheckPathExists = true,
                ValidateNames = true,
                OverwritePrompt = true,
                CreatePrompt = true
            };

            FileAccess.CheckSubdirectoryExists("Saves");
            FileAccess.CheckSubdirectoryExists("Backups");

            if (SaveFileName != "" && !saveAs)
            {
                if (FileAccess.CheckFileExists($"{Environment.CurrentDirectory}\\Saves\\{SaveFileName}"))
                {
                    if (_dialogService.ConfirmDialog("Overwrite existing save file?"))
                    {
                        string filePath = $"{Environment.CurrentDirectory}\\Saves\\{SaveFileName}";

                        XmlAccess.WriteObject(filePath, LoadedSave);
                    }
                }
                else
                {
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        XmlAccess.WriteObject(saveFileDialog.FileName, LoadedSave);
                        SaveFileName = saveFileDialog.SafeFileName;
                    }
                }
            }
            else
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    XmlAccess.WriteObject(saveFileDialog.FileName, LoadedSave);
                    SaveFileName = saveFileDialog.SafeFileName;
                }
            }
        }

        private void LoadPrompt()
        {
            FileAccess.CheckSubdirectoryExists("Saves");

            OpenFileDialog openFileDialog = new()
            {
                Filter = "Jumper Saves (*.xml)|*.xml",
                DefaultExt = "*.xml",
                InitialDirectory = Path.Combine(Environment.CurrentDirectory, "Saves"),
                CheckPathExists = true,
                ValidateNames = true,
                AddExtension = true,
                CheckFileExists = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SaveFileLoader saveFileLoader = new();
                saveFileLoader.LoadSave(openFileDialog.FileName, LoadedSave);
                SaveFileName = openFileDialog.SafeFileName;
            }
        }

        #endregion

        #region Commands
        [RelayCommand]
        private void NewJumper()
        {
            if (_dialogService.ConfirmDialog("Create new Jumper? (Will not save current Jumper data.)"))
            {
                SaveFileLoader saveFileLoader = new();
                saveFileLoader.NewSave(LoadedSave);
            }
        }

        [RelayCommand]
        private void SaveJumper() => SavePrompt(false);

        [RelayCommand]
        private void SaveJumperAs() => SavePrompt(true);

        [RelayCommand]
        private void LoadJumper() => LoadPrompt();

        [RelayCommand]
        private static void Quit() => Application.Current.Shutdown();

        [RelayCommand]
        private void SwitchTheme(string selection) => Theme = selection;

        [RelayCommand]
        private void OpenManual()
        {
            if (_dialogService.ConfirmDialog("Would you like to open the Github Wiki manual for this project? " +
                "(Opens link in default browser)"))
            {
                // TODO - Implement link to Wiki when it's done.
            }
        }

        #endregion
    }
}
