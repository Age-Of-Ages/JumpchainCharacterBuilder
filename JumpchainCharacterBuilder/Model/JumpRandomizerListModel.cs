﻿using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    public partial class JumpRandomizerList : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the list.
        /// </summary>
        [ObservableProperty]
        private string _listName = "";

        /// <summary>
        /// Represents the Jump entries contained within the list.
        /// </summary>
        [ObservableProperty]
        private List<JumpRandomizerEntry> _listEntries = [];

        public JumpRandomizerList()
        {
            
        }

        public JumpRandomizerList(JumpRandomizerList existingList)
        {
            ListName = existingList.ListName;

            foreach (JumpRandomizerEntry entry in existingList.ListEntries)
            {
                ListEntries.Add(new(entry));
            }
        }
    }
}
