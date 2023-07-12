using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JumpchainCharacterBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class JumpRandomizerSelectorViewModel : ViewModelBase
    {
        #region Fields
        [ObservableProperty]
        private ObservableCollection<JumpRandomizerList> _inactiveJumpRandomizerLists = new();
        [ObservableProperty]
        private JumpRandomizerList _activeJumpRandomizerList = new();
        [ObservableProperty]
        private List<JumpRandomizerEntry> _activeJumpPool = new();
        [ObservableProperty]
        private int _entriesToPull = 1;
        [ObservableProperty]
        private ObservableCollection<JumpRandomizerEntry> _winningEntries = new();

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
            LoadJumpLists();
        }
        #endregion

        #region Methods
        private void LoadJumpLists()
        {
            InactiveJumpRandomizerLists = new(RandomizeListAccess.ReadJumpListFile());
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

            if (ActiveJumpRandomizerList.ListEntries.Any())
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

            List<JumpRandomizerEntry> tempJumpPool = ActiveJumpPool.ToList();

            if (tempJumpPool.Any())
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

        #endregion
    }
}
