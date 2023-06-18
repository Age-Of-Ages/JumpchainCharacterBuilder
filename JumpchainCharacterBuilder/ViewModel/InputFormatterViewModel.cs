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
                string temporaryString = Regex.Replace(value, @"(\r\n)(?!\r\n)", " ");
                temporaryString = Regex.Replace(temporaryString, @"  ", "");

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
