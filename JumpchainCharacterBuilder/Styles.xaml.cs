﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace JumpchainCharacterBuilder
{
    public partial class Styles
    {
        private void TextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is TextBox textBoxTarget)
            {
                int caretIndex, cmdIndex;
                SpellingError spellingError;

                textBoxTarget.ContextMenu = GetContextMenu();
                caretIndex = textBoxTarget.CaretIndex;

                cmdIndex = 0;
                spellingError = textBoxTarget.GetSpellingError(caretIndex);
                if (spellingError != null )
                {
                    foreach (string suggestion in spellingError.Suggestions)
                    {
                        MenuItem item = new()
                        {
                            Header = suggestion,
                            FontWeight = FontWeights.Bold,
                            Command = EditingCommands.CorrectSpellingError,
                            CommandParameter = suggestion,
                            CommandTarget = textBoxTarget
                        };
                        textBoxTarget.ContextMenu.Items.Insert(cmdIndex, item);
                        cmdIndex++;
                    }
                    Separator separator1 = new();
                    textBoxTarget.ContextMenu.Items.Insert(cmdIndex, separator1);
                    cmdIndex++;
                    MenuItem ignoreItem = new()
                    {
                        Header = "Ignore",
                        Command = EditingCommands.IgnoreSpellingError,
                        CommandTarget = textBoxTarget
                    };
                    textBoxTarget.ContextMenu.Items.Insert(cmdIndex, ignoreItem);
                    cmdIndex++;
                    Separator separator2 = new();
                    textBoxTarget.ContextMenu.Items.Insert(cmdIndex, separator2);
                }

                MenuItem cutItem = new()
                {
                    Command = ApplicationCommands.Cut
                };
                MenuItem copyItem = new()
                {
                    Command = ApplicationCommands.Copy
                };
                MenuItem pasteItem = new()
                {
                    Command = ApplicationCommands.Paste
                };

                textBoxTarget.ContextMenu.Items.Add(cutItem);
                textBoxTarget.ContextMenu.Items.Add(copyItem);
                textBoxTarget.ContextMenu.Items.Add(pasteItem);
            }
        }

        private ContextMenu GetContextMenu()
        {
            ContextMenu menu = new();
            
            var contextMenuStyle = Application.Current.FindResource("ContextStyle") as Style;

            menu.Style = contextMenuStyle;

            return menu;
        }
    }
}
