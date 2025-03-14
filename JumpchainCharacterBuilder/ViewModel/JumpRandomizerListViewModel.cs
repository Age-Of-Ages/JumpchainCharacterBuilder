﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Attributes;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class JumpRandomizerListViewModel : ViewModelBase
    {
        // TODO - Add some sort of Jump List importer, if possible.
        #region Fields
        private readonly IDialogService _dialogService;
        [ObservableProperty]
        private bool _spellCheckEnabled = true;
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();

        [ObservableProperty]
        private ObservableCollection<JumpRandomizerList> _inactiveJumpRandomizerLists = [];
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteRandomizerListCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteRandomizerEntryCommand))]
        private JumpRandomizerList _activeJumpRandomizerList = new();
        [ObservableProperty]
        private ObservableCollection<JumpRandomizerEntry> _jumpRandomizerEntryList = [];
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NewRandomizerEntryCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteRandomizerEntryCommand))]
        private JumpRandomizerEntry _jumpRandomizerEntrySelection = new();
        [ObservableProperty]
        private ObservableCollection<JumpRandomizerEntry> _jumpRandomizerFilteredList = [];
        [ObservableProperty]
        private string _jumpRandomizerFilter = "";

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [ListTags(ErrorMessage = "Jump Randomizer entry names may not start with # or contain the following characters: " +
                                 "[, ], |")]
        private string _jumpSelectionName = "";

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [ValidURI(ErrorMessage = "Jump document URI is not well formed.")]
        private string _jumpSelectionUri = "About:Blank";

        #endregion

        #region Properties
        partial void OnActiveJumpRandomizerListChanged(JumpRandomizerList value)
        {
            if (value != null)
            {
                JumpRandomizerEntryList = new(value.ListEntries);

                if (value.ListEntries.Any())
                {
                    JumpRandomizerEntrySelection = value.ListEntries.First();
                }

                FilterJumpList();
            }
        }

        partial void OnJumpRandomizerEntrySelectionChanged(JumpRandomizerEntry value)
        {
            if (value != null)
            {
                JumpSelectionName = value.JumpName;
                JumpSelectionUri = value.JumpUri.ToString();
            }
        }

        partial void OnJumpSelectionNameChanged(string value)
        {
            if (!GetErrors(nameof(JumpSelectionName)).Any())
            {
                JumpRandomizerEntrySelection.JumpName = value;
            }
        }

        partial void OnJumpSelectionUriChanged(string value)
        {
            if (!GetErrors(nameof(JumpSelectionUri)).Any())
            {
                JumpRandomizerEntrySelection.JumpUri = new(value);
            }
            else if (value == "" || value == "about:Blank")
            {
                JumpRandomizerEntrySelection.JumpUri = new("About:Blank");
            }
        }

        partial void OnJumpRandomizerFilterChanged(string value)
        {
            FilterJumpList();
        }

        #endregion

        #region Constructor
        public JumpRandomizerListViewModel()
        {

        }

        public JumpRandomizerListViewModel(IDialogService dialogService)
        {
            Messenger.Register<SettingsLoadedMessage>(this, (r, m) =>
            {
                AppSettings = m.Value;

                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
            });
            Messenger.Register<SettingsChangedMessage>(this, (r, m) =>
            {
                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
            });

            LoadJumpLists();

            _dialogService = dialogService;
        }
        #endregion

        #region Methods
        private void LoadJumpLists()
        {
            InactiveJumpRandomizerLists = new(RandomizeListAccess.ReadJumpListFile());

            if (InactiveJumpRandomizerLists.Any())
            {
                ActiveJumpRandomizerList = InactiveJumpRandomizerLists.First();

                if (ActiveJumpRandomizerList.ListEntries.Count != 0)
                {
                    JumpRandomizerEntryList = new(ActiveJumpRandomizerList.ListEntries);
                    JumpRandomizerEntrySelection = ActiveJumpRandomizerList.ListEntries.First();
                }
            }

            DeleteRandomizerListCommand.NotifyCanExecuteChanged();
        }

        private void SaveJumpLists()
        {
            RandomizeListAccess.WriteJumpListFile([.. InactiveJumpRandomizerLists]);
        }

        private void FilterJumpList()
        {
            JumpRandomizerFilteredList = new(JumpRandomizerEntryList.Where(x => x.JumpName.Contains(JumpRandomizerFilter, System.StringComparison.CurrentCultureIgnoreCase)).ToList());
        }
        #endregion

        #region Commands
        [RelayCommand]
        private void NewRandomizerList()
        {
            JumpRandomizerList newList = new()
            {
                ListName = $"Jump randomizer list #{InactiveJumpRandomizerLists.Count + 1}"
            };

            InactiveJumpRandomizerLists.Add(newList);

            ActiveJumpRandomizerList = InactiveJumpRandomizerLists.Last();

            NewRandomizerEntryCommand.NotifyCanExecuteChanged();
            DeleteRandomizerEntryCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteRandomizerList))]
        private void DuplicateRandomizerList()
        {
            if (_dialogService.ConfirmDialog("Would you like to duplicate the current randomizer list?"))
            {
                JumpRandomizerList newList = new(ActiveJumpRandomizerList);

                newList.ListName = newList.ListName + " Copy";

                InactiveJumpRandomizerLists.Add(newList);

                ActiveJumpRandomizerList = InactiveJumpRandomizerLists.Last();

                NewRandomizerEntryCommand.NotifyCanExecuteChanged();
                DeleteRandomizerEntryCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand(CanExecute = nameof(CanDeleteRandomizerList))]
        private void DeleteRandomizerList()
        {
            if (_dialogService.ConfirmDialog("Are you sure that you want to delete this randomizer list?"))
            {
                InactiveJumpRandomizerLists.Remove(ActiveJumpRandomizerList);

                if (InactiveJumpRandomizerLists.Any())
                {
                    ActiveJumpRandomizerList = InactiveJumpRandomizerLists.First();
                }
                else
                {
                    JumpRandomizerEntryList.Clear();
                }

                NewRandomizerEntryCommand.NotifyCanExecuteChanged();
                DeleteRandomizerEntryCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteRandomizerList()
        {
            return InactiveJumpRandomizerLists.Any() && ActiveJumpRandomizerList != null;
        }

        [RelayCommand(CanExecute = nameof(CanAddNewJumpRandomizerEntry))]
        private void NewRandomizerEntry()
        {
            JumpRandomizerEntry newEntry = new()
            {
                JumpName = $"Jump #{ActiveJumpRandomizerList.ListEntries.Count + 1}"
            };

            ActiveJumpRandomizerList.ListEntries.Add(newEntry);
            JumpRandomizerEntryList.Add(newEntry);

            JumpRandomizerEntrySelection = JumpRandomizerEntryList.Last();

            FilterJumpList();

            NewRandomizerEntryCommand.NotifyCanExecuteChanged();
            DeleteRandomizerEntryCommand.NotifyCanExecuteChanged();
        }

        private bool CanAddNewJumpRandomizerEntry()
        {
            return ActiveJumpRandomizerList != null;
        }

        [RelayCommand(CanExecute = nameof(CanDeleteRandomizerEntry))]
        private void DeleteRandomizerEntry()
        {
            ActiveJumpRandomizerList.ListEntries.Remove(JumpRandomizerEntrySelection);
            JumpRandomizerEntryList.Remove(JumpRandomizerEntrySelection);

            FilterJumpList();

            DeleteRandomizerEntryCommand.NotifyCanExecuteChanged();
        }

        private bool CanDeleteRandomizerEntry()
        {
            return JumpRandomizerEntryList.Any() && JumpRandomizerEntrySelection != null;
        }

        [RelayCommand]
        private void SortJumpList()
        {
            if (_dialogService.ConfirmDialog("Would you like to sort the current list alphanumerically?"))
            {
                List<JumpRandomizerEntry> tempList = [.. JumpRandomizerEntryList.OrderBy(x => x.JumpName)];

                JumpRandomizerEntryList = new(tempList);
                ActiveJumpRandomizerList.ListEntries = tempList;
            }
        }

        [RelayCommand]
        private void SendChanges()
        {
            SaveJumpLists();
        }
        #endregion
    }
}
