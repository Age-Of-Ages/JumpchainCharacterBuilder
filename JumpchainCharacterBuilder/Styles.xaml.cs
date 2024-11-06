using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace JumpchainCharacterBuilder
{
    public partial class Styles
    {
        private void TextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is TextBox textBoxTarget)
            {
                PopulateTextBoxContextMenu(textBoxTarget);
            }
        }

        private void FormattedTextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is TextBox textBoxTarget)
            {
                PopulateTextBoxContextMenu(textBoxTarget);

                string? binding = BindingOperations.GetBinding(textBoxTarget, TextBox.TextProperty).Path.Path;
                if (binding != null)
                {
                    Separator separator3 = new();
                    textBoxTarget.ContextMenu.Items.Add(separator3);

                    bool textBoxFilled = !string.IsNullOrWhiteSpace(textBoxTarget.Text);

                    MenuItem formatItem = new()
                    {
                        Header = "Format string",
                        CommandParameter = binding,
                        IsEnabled = textBoxFilled
                    };

                    formatItem.SetBinding(MenuItem.CommandProperty, new Binding("FormatInputCommand"));

                    textBoxTarget.ContextMenu.Items.Add(formatItem);
                }
            }
        }

        private static ContextMenu GetContextMenu()
        {
            ContextMenu menu = new();

            var contextMenuStyle = Application.Current.FindResource("ContextStyle") as Style;

            menu.Style = contextMenuStyle;

            return menu;
        }

        private static void PopulateTextBoxContextMenu(TextBox textBoxTarget)
        {
            int caretIndex, cmdIndex;
            SpellingError spellingError;

            textBoxTarget.ContextMenu = GetContextMenu();
            caretIndex = textBoxTarget.CaretIndex;

            cmdIndex = 0;
            spellingError = textBoxTarget.GetSpellingError(caretIndex);
            if (spellingError != null)
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
}
