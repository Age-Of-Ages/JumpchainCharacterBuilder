using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using JumpchainCharacterBuilder.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class InputFormatterViewModel : ViewModelBase
    {
        #region Fields
        [ObservableProperty]
        private string _inputString = "";

        [ObservableProperty]
        private string _outputString = "";

        #endregion

        #region Properties
        partial void OnInputStringChanged(string value)
        {
            if (value != "")
            {
                // Copying from PDF files can cause issues with line-breaks being inserted in incorrect places, so these need to be removed.
                string temporaryString = Regex.Replace(value, @"(\r\n)(?!\r\n)", " ");
                // If a string with too many line-breaks is put through this formatter multiple times to fully correct it then extra spaces will appear.
                // These should be removed to preserve the tidiness of the string.
                temporaryString = Regex.Replace(temporaryString, @"  ", " ");

                OutputString = temporaryString;

            }
            else
            {
                OutputString = "";
            }
        }

        #endregion

        #region Constructor


        #endregion
    }
}
