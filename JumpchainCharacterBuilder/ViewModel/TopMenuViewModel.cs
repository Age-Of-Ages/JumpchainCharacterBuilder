using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using JumpchainCharacterBuilder.Services;
using Microsoft.Win32;
using System;
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
        private string _saveFileName = "";

        #endregion

        #region Properties


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

            _dialogService = dialogService;
        }

        #endregion

        #region Methods
        private void SavePrompt(bool saveAs)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Jumper Saves (*.xml)|*.xml";
            saveFileDialog.DefaultExt = "*.xml";
            saveFileDialog.AddExtension = true;
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.ValidateNames = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.CreatePrompt = true;

            XmlAccess.CheckSavesDirectoryExists();
            XmlAccess.CheckSaveBackupsDirectoryExists();

            if (SaveFileName != "" && !saveAs)
            {
                if (XmlAccess.CheckFileExists($"{Environment.CurrentDirectory}\\Saves\\{SaveFileName}"))
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Jumper Saves (*.xml)|*.xml";
            openFileDialog.DefaultExt = "*.xml";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.CheckPathExists = true;
            openFileDialog.ValidateNames = true;
            openFileDialog.AddExtension = true;
            openFileDialog.CheckFileExists = true;

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
        private void SaveJumper()
        {
            SavePrompt(false);
        }

        [RelayCommand]
        private void SaveJumperAs()
        {
            SavePrompt(true);
        }

        [RelayCommand]
        private void LoadJumper()
        {
            LoadPrompt();
        }

        [RelayCommand]
        private void Quit()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
