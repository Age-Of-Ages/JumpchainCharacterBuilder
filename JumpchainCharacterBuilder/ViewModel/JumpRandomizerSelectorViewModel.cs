using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using JumpchainCharacterBuilder.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class JumpRandomizerSelectorViewModel : ViewModelBase
    {
        #region Fields
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private AppSettingsModel _appSettings = new();
        [ObservableProperty]
        private bool _spellCheckEnabled = true;

        [ObservableProperty]
        private ObservableCollection<JumpRandomizerList> _inactiveJumpRandomizerLists = [];
        [ObservableProperty]
        private JumpRandomizerList _activeJumpRandomizerList = new();
        [ObservableProperty]
        private List<JumpRandomizerEntry> _activeJumpPool = [];
        [ObservableProperty]
        private int _entriesToPull = 1;
        [ObservableProperty]
        private ObservableCollection<JumpRandomizerEntry> _winningEntries = [];

        #endregion

        #region Properties
        partial void OnActiveJumpRandomizerListChanged(JumpRandomizerList value)
        {
            PopulateJumpPool();
        }
        #endregion

        #region Constructor
        public JumpRandomizerSelectorViewModel()
        {
            
        }

        public JumpRandomizerSelectorViewModel(IDialogService dialogService)
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
            InactiveJumpRandomizerLists = [.. RandomizeListAccess.ReadJumpListFile()];
            ActiveJumpRandomizerList = new();

            if (InactiveJumpRandomizerLists.Any())
            {
                ActiveJumpRandomizerList = InactiveJumpRandomizerLists.First();
            }

            PopulateJumpPool();
        }

        private void PopulateJumpPool()
        {
            ActiveJumpPool.Clear();

            if (ActiveJumpRandomizerList.ListEntries.Count != 0)
            {
                foreach (JumpRandomizerEntry entry in ActiveJumpRandomizerList.ListEntries)
                {
                    for (int i = 0; i < entry.JumpWeight; i++)
                    {
                        ActiveJumpPool.Add(entry);
                    }
                }
            }
        }

        private void DrawJumpWinners()
        {
            WinningEntries.Clear();

            List<JumpRandomizerEntry> tempJumpPool = [.. ActiveJumpPool];

            if (tempJumpPool.Count != 0)
            {
                if (tempJumpPool.Count >= EntriesToPull)
                {
                    Random rng = new();
                    int winnerIndex;

                    for (int i = 0; i < EntriesToPull; i++)
                    {
                        winnerIndex = rng.Next(tempJumpPool.Count);

                        WinningEntries.Add(tempJumpPool[winnerIndex]);

                        tempJumpPool.RemoveAt(winnerIndex);
                    }
                }
                else
                {
                    WinningEntries.Add(new()
                    {
                        JumpName = "Not enough entries available."
                    });
                }
            }
            else
            {
                WinningEntries.Add(new()
                {
                    JumpName = "No entries available."
                });
            }
        }

        #endregion

        #region Commands
        [RelayCommand]
        private void Draw()
        {
            DrawJumpWinners();
        }

        [RelayCommand]
        private static void CopyLink(Uri URI)
        {
            string uriString = URI.ToString();
            Clipboard.SetText(uriString);
        }


        [RelayCommand]
        private void AddJumpToChain(JumpRandomizerEntry entry)
        {
            if (_dialogService.ConfirmDialog("Add Jump to Jumpchain Overview?"))
            {
                Messenger.Send(new AddJumpToChainMessage(entry));
            }
        }
        #endregion
    }
}
