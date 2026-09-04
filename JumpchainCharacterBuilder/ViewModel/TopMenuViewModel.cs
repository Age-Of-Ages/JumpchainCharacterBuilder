using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<string> _recentlyAccessedFilePaths = [];

        [ObservableProperty]
        private string _theme = "Dark";

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

                LoadTheme();
                LoadRecentFilePaths();
            });
            Messenger.Register<SaveCommandMessage>(this, (r, m) =>
            {
                SavePrompt(false);
            });
            Messenger.Register<LoadCommandMessage>(this, (r, m) =>
            {
                LoadPrompt();
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

                        Messenger.Send(new SaveSucceededMessage(true));
                        AddNewRecentFile(filePath);
                    }
                    else
                    {
                        SavePrompt(true);
                    }
                }
                else
                {
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        XmlAccess.WriteObject(saveFileDialog.FileName, LoadedSave);
                        SaveFileName = saveFileDialog.SafeFileName;

                        Messenger.Send(new SaveSucceededMessage(true));
                        AddNewRecentFile(saveFileDialog.FileName);
                    }
                }
            }
            else
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    XmlAccess.WriteObject(saveFileDialog.FileName, LoadedSave);
                    SaveFileName = saveFileDialog.SafeFileName;

                    Messenger.Send(new SaveSucceededMessage(true));
                    AddNewRecentFile(saveFileDialog.FileName);
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

                AddNewRecentFile(openFileDialog.FileName);
            }
        }

        private void LoadTheme()
        {
            Theme = AppSettings.Theme;

            if (Theme == "Light")
            {
                LightThemeSelected = true;
                DarkThemeSelected = false;
            }
            else if (Theme == "Dark")
            {
                LightThemeSelected = false;
                DarkThemeSelected = true;
            }
        }

        private void LoadRecentFilePaths()
        {
            RecentlyAccessedFilePaths = [];

            foreach (string path in AppSettings.RecentlyAccessedFilePaths)
            {
                RecentlyAccessedFilePaths.Add(path);
            }
        }

        private void AddNewRecentFile(string filePath)
        {
            if (RecentlyAccessedFilePaths.Count >= 5)
            {
                RecentlyAccessedFilePaths.RemoveAt(4);
            }
            RecentlyAccessedFilePaths.Remove(filePath);
            RecentlyAccessedFilePaths.Insert(0, filePath);

            AppSettings.RecentlyAccessedFilePaths = [];

            foreach (string path in RecentlyAccessedFilePaths)
            {
                AppSettings.RecentlyAccessedFilePaths.Add(path);
            }

            CfgAccess.WriteCfgFile(AppSettings);
            Messenger.Send(new SettingsChangedMessage(true));
        }

        private void LoadRecentFile(string filePath)
        {
            if (!FileAccess.CheckFileExists(filePath))
            {
                _dialogService.NotificationDialog("Save not found, removing from list.");
                RecentlyAccessedFilePaths.Remove(filePath);
                AppSettings.RecentlyAccessedFilePaths.Remove(filePath);

                CfgAccess.WriteCfgFile(AppSettings);
                Messenger.Send(new SettingsChangedMessage(true));
            }
            else
            {
                if (_dialogService.ConfirmDialog("Would you like to save your current Jumper data before loading new data? (Unsaved data will be lost)"))
                {
                    SavePrompt(false);
                }
                SaveFileLoader saveFileLoader = new();
                saveFileLoader.LoadSave(filePath, LoadedSave);
                SaveFileName = Path.GetFileName(filePath);

                AddNewRecentFile(filePath);
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

                SaveFileName = "";
            }
        }

        [RelayCommand]
        private void SaveJumper() => SavePrompt(false);

        [RelayCommand]
        private void SaveJumperAs() => SavePrompt(true);

        [RelayCommand]
        private void LoadJumper() => LoadPrompt();

        [RelayCommand]
        private void OpenRecentFile(string filePath) => LoadRecentFile(filePath);

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

            }
        }

        #endregion
    }
}
