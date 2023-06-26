using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class ExportViewModel : ViewModelBase
    {

        #region Fields
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private Options _loadedOptions = new();
        [ObservableProperty]
        private ExportOptions _loadedExportOptions = new();
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();

        [ObservableProperty]
        private ObservableCollection<Character> _characterList = new();
        [ObservableProperty]
        private Character _characterSelection = new();
        [ObservableProperty]
        private int _characterSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<Jump> _jumpList = new();
        [ObservableProperty]
        private Jump _jumpSelection = new();

        [ObservableProperty]
        private int _exportTabIndex = 0;

        [ObservableProperty]
        private ObservableCollection<string> _exportModeList = new()
        {
            "Generic",
            "BBCode",
            "Markdown"
        };
        [ObservableProperty]
        private string _exportModeSelection = "BBCode";

        [ObservableProperty]
        private bool _modeSelectedGeneric = false;
        [ObservableProperty]
        private bool _modeSelectedBBCode = true;
        [ObservableProperty]
        private bool _modeSelectedMarkdown = false;

        [ObservableProperty]
        private ObservableCollection<char> _sectionSeparatorOptions = new()
        {
            '=',
            '-',
            '_',
            '~'
        };
        [ObservableProperty]
        private ObservableCollection<string> _budgetEnclosingOptions = new()
        {
            "[]",
            "()",
            "{}",
            "<>",
            "||"
        };
        [ObservableProperty]
        private ObservableCollection<char> _budgetSeparatorOptions = new()
        {
            '/',
            '\\',
            '|',
            ':'
        };
        [ObservableProperty]
        private ObservableCollection<string> _budgetOrderFormat = new()
        {
            "Name [Cost/Budget]",
            "Name [Cost/Section Subtotal]",
            "Name [Cost]",
            "Name [Budget]",
            "Name [Section Subtotal]"
        };

        [ObservableProperty]
        private ObservableCollection<ExportFormatToggle> _buildSectionList = new()
        {
            new("Bank Details", true),
            new("Point Summary", false),
            new("Jump Details", true),
            new("Jump Duration", true),
            new("Origin", true),
            new("Age", true),
            new("Gender", true),
            new("Location", true),
            new("Species", true ),
            new("Misc Origin Details", true),
            new("Perks", true),
            new("Items", true),
            new("Import Options", true),
            new("Drawbacks", true),
            new("Scenarios", true),
            new("Supplements", true)
        };
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MoveBuildSectionUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveBuildSectionDownCommand))]
        private int _buildSectionSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<ExportFormatToggle> _profileSectionList = new()
        {
            new("Profile", true),
            new("Alt Forms", true),
            new("Perks", true),
            new("Items", true),
            new("Attributes", true),
            new("Skills", true),
            new("Learning Rates", true)
        };
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MoveProfileSectionUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveProfileSectionDownCommand))]
        private int _profileSectionSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ExportFormatToggle> _profileSubsectionList = new()
        {
            new("Biography", true),
            new("Physical Characteristics", true),
            new("Personality", true),
            new("Physical Description", true),
            new("Traits", true)
        };
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MoveProfileSubsectionUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveProfileSubsectionDownCommand))]
        private int _profileSubsectionSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<ExportFormatToggle> _genericWarehouseSectionList = new()
        {
            new("Point Summary", true),
            new("Block Description", true),
            new("Purchases", true),
            new("Additions", true),
            new("Limitations", true)
        };
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MoveWarehouseSectionUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveWarehouseSectionDownCommand))]
        private int _genericWarehouseSectionSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<ExportFormatToggle> _personalRealitySectionList = new()
        {
            new("Point Summary", true),
            new("Core Mode", true),
            new("Purchases", true),
            new("Additions", true),
            new("Limitations", true)
        };
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MoveWarehouseSectionUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveWarehouseSectionDownCommand))]
        private int _personalRealitySectionSelectionIndex = 0;

        [ObservableProperty]
        private bool _genericWarehouseSelected = true;
        [ObservableProperty]
        private bool _personalRealitySelected = false;

        [ObservableProperty]
        private ObservableCollection<ExportFormatToggle> _bodyModSectionList = new()
        {
            new("Point Summary", true),
            new("Supplement Details", true),
            new("Perks", true),
            new("Additions", true),
            new("Drawbacks", true)
        };
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MoveBodyModSectionUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveBodyModSectionDownCommand))]
        private int _bodyModSectionSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<ExportFormatToggle> _drawbackSupplementSectionList = new()
        {
            new("Point Summary", true),
            new("Supplement Details", true),
            new("House Rules", true),
            new("Drawbacks", true)
        };
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MoveDrawbackSupplementSectionUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveDrawbackSupplementSectionDownCommand))]
        private int _drawbackSupplementSectionSelectionIndex = 0;
        #endregion

        #region Properties
        partial void OnExportModeSelectionChanged(string value)
        {
            switch (value)
            {
                case "Generic":
                    ModeSelectedGeneric = true;
                    ModeSelectedBBCode = false;
                    ModeSelectedMarkdown = false;
                    break;
                case "BBCode":
                    ModeSelectedGeneric = false;
                    ModeSelectedBBCode = true;
                    ModeSelectedMarkdown = false;
                    break;
                case "Markdown":
                    ModeSelectedGeneric = false;
                    ModeSelectedBBCode = false;
                    ModeSelectedMarkdown = true;
                    break;
                default:
                    break;
            }

            LoadedExportOptions.ExportMode = value;
        }

        partial void OnExportTabIndexChanged(int value)
        {
            switch (value)
            {
                case 1:
                    LoadCharacterList();
                    LoadJumpList();
                    break;
                case 2:
                    LoadCharacterList();
                    break;
                case 3:
                    LoadCharacterList();
                    LoadJumpList();
                    break;
                case 4:
                    LoadWarehouseSettings();
                    break;
                case 5:
                    LoadCharacterList();
                    break;
            }

        }
        #endregion

        #region Constructor

        public ExportViewModel()
        {

        }

        public ExportViewModel(IDialogService dialogService)
        {
            Messenger.Register<SaveDataChangedMessage>(this, (r, m) =>
            {
                LoadedOptions = LoadedSave.Options;
                LoadedExportOptions = LoadedOptions.ExportOptions;

                LoadCharacterList();
                LoadJumpList();
                LoadExportConfig();
            });
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;
                LoadedOptions = LoadedSave.Options;
                LoadedExportOptions = LoadedOptions.ExportOptions;

                LoadCharacterList();
                LoadJumpList();
                LoadExportConfig();
            });
            Messenger.Register<SupplementChangedMessage>(this, (r, m) =>
            {
                LoadWarehouseSettings();
            });
            Messenger.Register<SettingsLoadedMessage>(this, (r, m) =>
            {
                AppSettings = m.Value;
            });

            _dialogService = dialogService;
        }

        #endregion

        #region Methods
        private void LoadCharacterList()
        {
            CharacterList.Clear();

            foreach (Character character in LoadedSave.CharacterList)
            {
                CharacterList.Add(character);
            }

            if (CharacterList.Any())
            {
                CharacterSelection = CharacterList.First();
                CharacterSelectionIndex = 0;
            }
        }

        private void LoadJumpList()
        {
            JumpList.Clear();

            foreach (Jump jump in LoadedSave.JumpList)
            {
                JumpList.Add(jump);
            }

            if (JumpList.Any())
            {
                JumpSelection = JumpList.First();
            }
        }

        private void LoadExportConfig()
        {
            ExportModeSelection = LoadedExportOptions.ExportMode;
            BuildSectionList.Clear();
            ProfileSectionList.Clear();
            ProfileSubsectionList.Clear();
            GenericWarehouseSectionList.Clear();
            PersonalRealitySectionList.Clear();
            BodyModSectionList.Clear();
            DrawbackSupplementSectionList.Clear();

            foreach (ExportFormatToggle section in LoadedExportOptions.BuildSectionList)
            {
                BuildSectionList.Add(section);
            }

            foreach (ExportFormatToggle section in LoadedExportOptions.ProfileSectionList)
            {
                ProfileSectionList.Add(section);
            }

            foreach (ExportFormatToggle section in LoadedExportOptions.ProfileSubsectionList)
            {
                ProfileSubsectionList.Add(section);
            }

            foreach (ExportFormatToggle section in LoadedExportOptions.GenericWarehouseSectionList)
            {
                GenericWarehouseSectionList.Add(section);
            }

            foreach (ExportFormatToggle section in LoadedExportOptions.PersonalRealitySectionList)
            {
                PersonalRealitySectionList.Add(section);
            }

            foreach (ExportFormatToggle section in LoadedExportOptions.BodyModSectionList)
            {
                BodyModSectionList.Add(section);
            }

            foreach (ExportFormatToggle section in LoadedExportOptions.DrawbackSupplementSectionList)
            {
                DrawbackSupplementSectionList.Add(section);
            }
        }

        private void LoadWarehouseSettings()
        {
            switch (LoadedOptions.CosmicWarehouseSetting)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    GenericWarehouseSelected = true;
                    PersonalRealitySelected = false;
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    GenericWarehouseSelected = false;
                    PersonalRealitySelected = true;
                    break;
            }
        }

        private string FormatCurrencyAbbreviation(string abbreviation)
        {
            string output = "";

            switch (LoadedExportOptions.ExportMode)
            {
                case "Generic":
                    if (LoadedExportOptions.GenericFormattingOptions[2].Enabled)
                    {
                        output = $" {abbreviation}";
                    }
                    break;
                case "BBCode":
                    if (LoadedExportOptions.BBCodeFormattingOptions[8].Enabled)
                    {
                        output = $" {abbreviation}";
                    }
                    break;
                case "Markdown":
                    if (LoadedExportOptions.MarkdownFormattingOptions[7].Enabled)
                    {
                        output = $" {abbreviation}";
                    }
                    break;
                default:
                    break;
            }


            return output;
        }

        private string FormatSectionTitle(string title)
        {
            switch (LoadedExportOptions.ExportMode)
            {
                case "BBCode":
                    if (LoadedExportOptions.BBCodeFormattingOptions[0].Enabled)
                    {
                        title = "[B]" + title + "[/B]";
                    }
                    if (LoadedExportOptions.BBCodeFormattingOptions[1].Enabled)
                    {
                        title = "[I]" + title + "[/I]";
                    }
                    if (LoadedExportOptions.BBCodeFormattingOptions[2].Enabled)
                    {
                        title = "[U]" + title + "[/U]";
                    }
                    if (LoadedExportOptions.BBCodeFormattingOptions[3].Enabled)
                    {
                        title = "[CENTER]" + title + "[/CENTER]";
                    }
                    break;
                case "Markdown":
                    if (LoadedExportOptions.MarkdownFormattingOptions[0].Enabled && LoadedExportOptions.MarkdownFormattingOptions[1].Enabled)
                    {
                        title = "***" + title + "***";
                    }
                    else
                    {
                        if (LoadedExportOptions.MarkdownFormattingOptions[0].Enabled)
                        {
                            title = "**" + title + "**";
                        }
                        if (LoadedExportOptions.MarkdownFormattingOptions[1].Enabled)
                        {
                            title = "**" + title + "*";
                        }
                    }
                    if (LoadedExportOptions.MarkdownFormattingOptions[2].Enabled)
                    {
                        title = "#" + title;
                    }
                    break;
                default:
                    break;
            }

            return title;
        }

        private static void FormatDocumentDetails(DocumentDetail document, List<string> output)
        {
            if (document.Name != "")
            {
                output.Add("Supplement name: " + document.Name);
            }
            if (document.Version != "")
            {
                output.Add("Version: " + document.Version);
            }
            if (document.Author != "")
            {
                output.Add("Author: " + document.Author);
            }
            if (document.Source != "")
            {
                output.Add("Source: " + document.Source);
            }
        }

        private static string FormatPointSummary(int baseBudget, Dictionary<string, int> additionalValues, string currencyAbbreviation)
        {
            int subtotal = 0;

            subtotal += baseBudget;

            foreach (int value in additionalValues.Values)
            {
                subtotal += value;
            }

            string line = $"Point Total: {subtotal}{currencyAbbreviation} | {baseBudget} (Base)";

            foreach (KeyValuePair<string, int> value in additionalValues)
            {
                if (value.Value < 0)
                {
                    line += $" - {value.Value} ({value.Key})";
                }
                else
                {
                    line += $" + {value.Value} ({value.Key})";
                }
            }

            return line;
        }

        private string FormatBudgetLastHalf(int value, int budget, int sectionSubtotal, string currencyAbbreviation)
        {
            char leftBracket = LoadedExportOptions.BudgetEnclosingFormat[0];
            char rightBracket = LoadedExportOptions.BudgetEnclosingFormat[1];
            char budgetSeparator = LoadedExportOptions.BudgetSeparatorFormat;

            return LoadedExportOptions.BudgetFormat switch
            {
                0 => $"{leftBracket}{value}{budgetSeparator}{budget}{currencyAbbreviation}{rightBracket}",
                1 => $"{leftBracket}{value}{budgetSeparator}{sectionSubtotal}{currencyAbbreviation}{rightBracket}",
                2 => $"{leftBracket}{value}{currencyAbbreviation}{rightBracket}",
                3 => $"{leftBracket}{budget}{currencyAbbreviation}{rightBracket}",
                4 => $"{leftBracket}{sectionSubtotal}{currencyAbbreviation}{rightBracket}",
                _ => $"{leftBracket}{value}{budgetSeparator}{budget}{currencyAbbreviation}{rightBracket}",
            };
        }

        private string FormatUDSBudgetLastHalf(Dictionary<string, string> budgetLastHalves, Dictionary<string, int> values, Dictionary<string, int> budgets)
        {
            char leftBracket = LoadedExportOptions.BudgetEnclosingFormat[0];
            char rightBracket = LoadedExportOptions.BudgetEnclosingFormat[1];
            char budgetSeparator = LoadedExportOptions.BudgetSeparatorFormat;
            string currencyAbbreviation = "";

            foreach (string key in budgetLastHalves.Keys)
            {
                switch (key)
                {
                    case "Choice Points":
                        currencyAbbreviation = FormatCurrencyAbbreviation("CP");
                        break;
                    case "Warehouse Points":
                        currencyAbbreviation = FormatCurrencyAbbreviation("WP");
                        break;
                    case "Companion Choice Points":
                        currencyAbbreviation = FormatCurrencyAbbreviation("Companion CP");
                        break;
                    case "Item Choice Points":
                        currencyAbbreviation = FormatCurrencyAbbreviation("Item-only CP");
                        break;
                    default:
                        break;
                }

                budgetLastHalves[key] = LoadedExportOptions.BudgetFormat switch
                {
                    0 => $"{leftBracket}{values[key]}{budgetSeparator}{budgets[key]}{currencyAbbreviation}{rightBracket}",
                    1 => $"{leftBracket}{values[key]}{budgetSeparator}{budgets[key]}{currencyAbbreviation}{rightBracket}",
                    2 => $"{leftBracket}{values[key]}{currencyAbbreviation}{rightBracket}",
                    3 => $"{leftBracket}{budgets[key]}{currencyAbbreviation}{rightBracket}",
                    4 => $"{leftBracket}{budgets[key]}{currencyAbbreviation}{rightBracket}",
                    _ => $"{leftBracket}{values[key]}{budgetSeparator}{budgets[key]}{currencyAbbreviation}{rightBracket}",
                };
            }

            return $"{budgetLastHalves["Choice Points"]} " +
                $"{budgetLastHalves["Warehouse Points"]} " +
                $"{budgetLastHalves["Companion Choice Points"]} " +
                $"{budgetLastHalves["Companion Choice Points"]}";
        }

        private void FormatPricedDataLine(string name, string description, string budgetLastHalf, List<string> output, int cost = 1, string reward = "")
        {
            string line;
            switch (LoadedExportOptions.ExportMode)
            {
                case "Generic":
                    if (cost == 0)
                    {
                        budgetLastHalf = "(Free)";
                    }
                    if (LoadedExportOptions.ReverseBudgetFormat)
                    {
                        line = $"{budgetLastHalf} {name}";
                    }
                    else
                    {
                        line = $"{name} {budgetLastHalf}";
                    }

                    if (LoadedExportOptions.GenericFormattingOptions[0].Enabled)
                    {
                        if (LoadedExportOptions.GenericFormattingOptions[1].Enabled)
                        {
                            line += $" - {description}";

                            if (reward != "")
                            {
                                line += $" - {reward}";
                            }
                            output.Add(line);
                        }
                        else
                        {
                            output.Add(line);

                            output.Add(description);
                            if (reward != "")
                            {
                                output.Add(reward);
                            }
                        }
                    }
                    break;
                case "BBCode":
                    if (cost == 0)
                    {
                        budgetLastHalf = "(Free)";
                    }
                    if (LoadedExportOptions.ReverseBudgetFormat)
                    {
                        line = $"{budgetLastHalf} {name}";
                    }
                    else
                    {
                        line = $"{name} {budgetLastHalf}";
                    }

                    if (LoadedExportOptions.BBCodeFormattingOptions[4].Enabled)
                    {
                        line = "[SPOILER=\"" + line + "\"]";
                    }
                    else if (LoadedExportOptions.BBCodeFormattingOptions[5].Enabled)
                    {
                        line = FormatSectionTitle(line);
                    }

                    if (LoadedExportOptions.BBCodeFormattingOptions[6].Enabled)
                    {
                        if (LoadedExportOptions.BBCodeFormattingOptions[7].Enabled)
                        {
                            line += $" - {description}";

                            if (reward != "")
                            {
                                line += $" - {reward}";
                            }

                            if (LoadedExportOptions.BBCodeFormattingOptions[4].Enabled)
                            {
                                line += " [/SPOILER]";
                            }

                            output.Add(line);
                        }
                        else
                        {
                            output.Add(line);

                            if (LoadedExportOptions.BBCodeFormattingOptions[4].Enabled)
                            {
                                output.Add(description);
                                if (reward != "")
                                {
                                    output.Add(reward + " [/SPOILER]");
                                }
                                else
                                {
                                    output.Add("[/SPOILER]");
                                }
                            }
                            else
                            {
                                output.Add(description);
                                if (reward != "")
                                {
                                    output.Add(reward);
                                }
                            }

                        }
                    }
                    break;
                case "Markdown":
                    if (cost == 0)
                    {
                        budgetLastHalf = "(Free)";
                    }
                    if (LoadedExportOptions.ReverseBudgetFormat)
                    {
                        line = $"{budgetLastHalf} {name}";
                    }
                    else
                    {
                        line = $"{name} {budgetLastHalf}";
                    }

                    if (LoadedExportOptions.MarkdownFormattingOptions[3].Enabled)
                    {
                        line = FormatSectionTitle(line);
                    }

                    if (LoadedExportOptions.MarkdownFormattingOptions[5].Enabled)
                    {
                        if (LoadedExportOptions.MarkdownFormattingOptions[6].Enabled)
                        {
                            line += $" - {description}";

                            if (reward != "")
                            {
                                line += $" - {reward}";
                            }

                            output.Add(line);
                        }
                        else
                        {
                            output.Add(line);

                            output.Add(description);
                            if (reward != "")
                            {
                                output.Add(reward);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void FormatNonPricedDataLine(string name, string description, List<string> output, string sourceCharacter = "", string sourceJump = "")
        {
            string line;

            switch (LoadedExportOptions.ExportMode)
            {
                case "Generic":
                    line = name;

                    if (LoadedExportOptions.GenericFormattingOptions[0].Enabled)
                    {
                        if (LoadedExportOptions.GenericFormattingOptions[1].Enabled)
                        {
                            line += $" - {description}";

                            output.Add(line);
                        }
                        else
                        {
                            output.Add(line);

                            output.Add(description);
                        }
                    }

                    if (sourceJump != "")
                    {
                        FormatBlankLine(output);
                        output.Add("Source Jump: " + sourceJump);
                    }

                    if (sourceCharacter != "")
                    {
                        FormatBlankLine(output);
                        output.Add("Source Character: " + sourceCharacter);
                    }
                    break;
                case "BBCode":
                    line = name;

                    if (LoadedExportOptions.BBCodeFormattingOptions[4].Enabled)
                    {
                        line = "[SPOILER=\"" + line + "\"]";
                    }
                    else if (LoadedExportOptions.BBCodeFormattingOptions[5].Enabled)
                    {
                        line = FormatSectionTitle(line);
                    }

                    if (LoadedExportOptions.BBCodeFormattingOptions[6].Enabled)
                    {
                        if (LoadedExportOptions.BBCodeFormattingOptions[7].Enabled)
                        {
                            line += $" - {description}";

                            output.Add(line);
                        }
                        else
                        {
                            output.Add(line);

                            output.Add(description);

                        }
                    }

                    if (sourceJump != "")
                    {
                        FormatBlankLine(output);
                        output.Add("Source Jump: " + sourceJump);
                    }

                    if (sourceCharacter != "")
                    {
                        FormatBlankLine(output);
                        output.Add("Source Character: " + sourceCharacter);
                    }

                    if (LoadedExportOptions.BBCodeFormattingOptions[4].Enabled)
                    {
                        output.Add("[/SPOILER]");
                    }
                    break;
                case "Markdown":
                    line = name;

                    if (LoadedExportOptions.MarkdownFormattingOptions[3].Enabled)
                    {
                        line = FormatSectionTitle(line);
                    }

                    if (LoadedExportOptions.MarkdownFormattingOptions[5].Enabled)
                    {
                        if (LoadedExportOptions.MarkdownFormattingOptions[6].Enabled)
                        {
                            line += $" - {description}";

                            output.Add(line);
                        }
                        else
                        {
                            output.Add(line);

                            output.Add(description);
                        }
                    }

                    if (sourceJump != "")
                    {
                        FormatBlankLine(output);
                        output.Add("Source Jump: " + sourceJump);
                    }

                    if (sourceCharacter != "")
                    {
                        FormatBlankLine(output);
                        output.Add("Source Character: " + sourceCharacter);
                    }
                    break;
                default:
                    break;
            }
        }

        private void FormatOpeningLines(List<string> output, string title)
        {
            switch (LoadedExportOptions.ExportMode)
            {
                case "Generic":
                    output.Add(title);
                    FormatBlankLine(output);
                    FormatBlankLine(output);
                    break;
                case "BBCode":
                    if (LoadedExportOptions.BBCodeFormattingOptions[9].Enabled)
                    {
                        output.Add($"[SPOILER=\"{title}\"]");
                    }
                    break;
                case "Markdown":
                    output.Add($"# {title}");
                    break;
                default:
                    break;
            }
        }

        private void FormatClosingLines(List<string> output)
        {
            if (LoadedExportOptions.ExportMode == "BBCode" && LoadedExportOptions.BBCodeFormattingOptions[9].Enabled)
            {
                FormatBlankLine(output);
                output.Add($"[/SPOILER]");
            }
        }

        private void FormatSectionEnding(List<string> output, bool sectionEnabled, char sectionSeparator)
        {
            string line;

            switch (LoadedExportOptions.ExportMode)
            {
                case "Generic":
                    if (LoadedExportOptions.GenericFormattingOptions[3].Enabled && sectionEnabled)
                    {
                        line = "";

                        FormatBlankLine(output);

                        for (int i = 0; i < 10; i++)
                        {
                            line += $"{sectionSeparator}";
                        }

                        output.Add(line);

                        FormatBlankLine(output);
                    }
                    break;
                case "BBCode":
                    if (LoadedExportOptions.BBCodeFormattingOptions[10].Enabled && sectionEnabled)
                    {
                        FormatBlankLine(output);

                        if (LoadedExportOptions.BBCodeFormattingOptions[11].Enabled)
                        {
                            output.Add("[HR=3][/HR]");
                        }
                        else
                        {
                            line = "";

                            for (int i = 0; i < 10; i++)
                            {
                                line += $"{sectionSeparator}";
                            }

                            output.Add(line);
                        }

                        FormatBlankLine(output);
                    }
                    break;
                case "Markdown":
                    if (LoadedExportOptions.MarkdownFormattingOptions[8].Enabled && sectionEnabled)
                    {
                        FormatBlankLine(output);

                        if (LoadedExportOptions.MarkdownFormattingOptions[9].Enabled)
                        {
                            output.Add("***");
                        }
                        else
                        {
                            line = "";

                            for (int i = 0; i < 10; i++)
                            {
                                line += $"{sectionSeparator}";
                            }

                            output.Add(line);
                        }

                        FormatBlankLine(output);
                    }
                    break;
                default:
                    break;
            }
        }

        private void FormatBlankLine(List<string> output)
        {
            if (LoadedExportOptions.ExportMode == "Markdown")
            {
                output.Add("");
                output.Add("&nbsp;");
                output.Add("");
            }
            else
            {
                output.Add("");
            }
        }

        private int CalculateJumpWP(WarehouseUniversal warehouse)
        {
            int jumps = LoadedSave.JumpList.Last().JumpNumber - warehouse.SupplementDelay;
            if (jumps < 0)
            {
                jumps = 0;
            }

            int deposits = jumps / warehouse.IncrementalInterval;

            return deposits * warehouse.IncrementalBudget;
        }

        private int CalculateInvestmentWP(WarehouseUniversal warehouse)
        {
            int total = 0;

            foreach (Jump jump in LoadedSave.JumpList)
            {
                total += jump.Build[0].WarehouseInvestment;
            }

            if (warehouse.TotalInvestment != (total / warehouse.InvestmentRatio))
            {
                warehouse.TotalInvestment = (total / warehouse.InvestmentRatio);
            }

            return warehouse.TotalInvestment;
        }

        private static int CalculateLimitationWP(WarehouseUniversal warehouse)
        {
            int total = 0;

            foreach (SupplementDrawbackModel limitation in warehouse.Limitations)
            {
                total += limitation.Value;
            }

            return total;
        }

        private int CalculateJumpBP(int characterIndex)
        {
            int total;
            int jumps;
            int deposits;

            switch (LoadedOptions.BodyModSetting)
            {
                case Options.BodyModSupplements.Generic:
                    jumps = LoadedSave.JumpList.Last().JumpNumber - CharacterList[characterIndex].BodyMod.SupplementDelay;
                    if (jumps < 0)
                    {
                        jumps = 0;
                    }

                    deposits = jumps / LoadedSave.GenericBodyMod.IncrementalInterval;

                    total = (deposits * LoadedSave.GenericBodyMod.IncrementalBudget);
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    jumps = LoadedSave.JumpList.Last().JumpNumber - CharacterList[characterIndex].BodyMod.SupplementDelay;
                    if (jumps < 0)
                    {
                        jumps = 0;
                    }

                    deposits = jumps / LoadedSave.SBBodyMod.IncrementalInterval;

                    total = (deposits * LoadedSave.SBBodyMod.IncrementalBudget);
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    jumps = LoadedSave.JumpList.Last().JumpNumber - CharacterList[characterIndex].BodyMod.SupplementDelay;
                    if (jumps < 0)
                    {
                        jumps = 0;
                    }

                    deposits = jumps / LoadedSave.EssentialBodyMod.IncrementalInterval;

                    total = (deposits * LoadedSave.EssentialBodyMod.IncrementalBudget);
                    break;
                default:
                    total = 0;
                    break;
            }

            return total;
        }

        private int CalculateInvestmentBP(int characterIndex)
        {
            int total = 0;

            foreach (Jump jump in LoadedSave.JumpList)
            {
                if (jump.Build.Count >= characterIndex + 1 && jump.Build[characterIndex] != null)
                {
                    total += jump.Build[characterIndex].BodyModInvestment;
                }
            }

            return total;
        }

        private int CalculateDrawbackBP(BodyModUniversal bodyMod)
        {
            int total = 0;

            switch (LoadedOptions.BodyModSetting)
            {
                case Options.BodyModSupplements.Generic:
                    foreach (SupplementDrawbackModel drawback in bodyMod.Limitations)
                    {
                        if (drawback.Category == "Generic")
                        {
                            total += drawback.Value;
                        }
                    }
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    foreach (SupplementDrawbackModel drawback in bodyMod.EBMDrawbackList)
                    {
                        total += drawback.Value;
                    }
                    break;
                default:
                    break;
            }

            return total;
        }

        private int CalculateQuestBP(int characterIndex)
        {
            int result;

            int MinorBP = CharacterList[characterIndex].BodyMod.EBMMinorQuests * 50;
            int MajorBP = CharacterList[characterIndex].BodyMod.EBMMajorQuests * 100;

            result = MinorBP + MajorBP;

            return result;
        }

        private void ExportBuild(Jump jump, int characterIndex)
        {
            JumpBuild build = jump.Build[characterIndex];
            DrawbackSupplementUniversal drawbackSupplement = new();
            int jumpIndex = LoadedSave.JumpList.IndexOf(jump);

            switch (LoadedOptions.DrawbackSupplementSetting)
            {
                case Options.DrawbackSupplements.Generic:
                    drawbackSupplement = LoadedSave.GenericDrawbackSupplement;
                    break;
                case Options.DrawbackSupplements.UDS:
                    drawbackSupplement = LoadedSave.UniversalDrawbackSupplement;
                    break;
                case Options.DrawbackSupplements.UU:
                    drawbackSupplement = LoadedSave.UUSupplement;
                    break;
                default:
                    break;
            }


            char leftBracket = LoadedExportOptions.BudgetEnclosingFormat[0];
            char rightBracket = LoadedExportOptions.BudgetEnclosingFormat[1];
            char budgetSeparator = LoadedExportOptions.BudgetSeparatorFormat;
            char sectionSeparator = LoadedExportOptions.SectionSeparator;
            string budgetLastHalf = "";
            string currencyAbbreviation;

            List<string> output = new();
            string line = "";

            List<int> budget = new();
            List<int> stipend;
            int sectionSubtotal;

            int importStipend = 0;

            int drawbackSupplementCP = 0;
            int drawbackSupplementItemCP = 0;
            int drawbackSupplementCompanionCP = 0;

            bool isGauntlet = jump.IsGauntlet;
            bool supplementPointsAllowed = true;
            bool halvedSupplementPoints = false;
            if (isGauntlet && !drawbackSupplement.AllowedDuringGauntlets)
            {
                supplementPointsAllowed = false;
            }
            if (drawbackSupplement.HalvedPointsDuringGauntlets)
            {
                halvedSupplementPoints = true;
            }

            if (isGauntlet)
            {
                if (supplementPointsAllowed)
                {
                    foreach (DrawbackSupplementPurchase drawback in drawbackSupplement.Purchases)
                    {
                        if (drawback.Revoke == 0 || drawback.Revoke > jumpIndex)
                        {
                            if (jumpIndex >= drawback.Suspend.Count)
                            {
                                ListValidationClass.CheckDrawbackSuspendCount(drawback, jumpIndex + 1);
                            }
                            if (!drawback.Suspend[jumpIndex])
                            {
                                if (drawback.ApplyGauntlet)
                                {
                                    if (halvedSupplementPoints)
                                    {
                                        drawbackSupplementCP += drawback.ValueChoicePoints / 2;
                                        drawbackSupplementItemCP += drawback.ValueItemPoints / 2;
                                        drawbackSupplementCompanionCP += drawback.ValueCompanionPoints / 2;
                                    }
                                    else
                                    {
                                        drawbackSupplementCP += drawback.ValueChoicePoints;
                                        drawbackSupplementItemCP += drawback.ValueItemPoints;
                                        drawbackSupplementCompanionCP += drawback.ValueCompanionPoints;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DrawbackSupplementPurchase drawback in drawbackSupplement.Purchases)
                {
                    if (drawback.Revoke == 0 || drawback.Revoke > jumpIndex)
                    {
                        if (jumpIndex >= drawback.Suspend.Count)
                        {
                            ListValidationClass.CheckDrawbackSuspendCount(drawback, jumpIndex + 1);
                        }
                        if (!drawback.Suspend[jumpIndex])
                        {
                            drawbackSupplementCP += drawback.ValueChoicePoints;
                            drawbackSupplementItemCP += drawback.ValueItemPoints;
                            drawbackSupplementCompanionCP += drawback.ValueCompanionPoints;
                        }
                    }
                }
            }

            if (characterIndex > 0)
            {
                foreach (JumpBuild jumpBuild in jump.Build)
                {
                    foreach (CompanionPurchase companionPurchase in jumpBuild.CompanionPurchase)
                    {
                        importStipend += companionPurchase.CompanionImportDetails[characterIndex - 1].CompanionOptionValue;
                    }
                }

                budget = build.PointStipend;
                budget[0] += drawbackSupplementCompanionCP;
            }
            else
            {
                foreach (Currency currency in jump.Currencies)
                {
                    budget.Add(currency.CurrencyBudget);
                }

                stipend = build.PointStipend;

                for (int i = 0; i < budget.Count; i++)
                {
                    budget[i] += stipend[i];
                }

                budget[0] += drawbackSupplementCP;
            }

            FormatOpeningLines(output, $"{jump.Name} Build");

            foreach (ExportFormatToggle section in LoadedExportOptions.BuildSectionList)
            {
                switch (section.Name)
                {
                    case "Bank Details":
                        if (section.Enabled)
                        {
                            output.Add(FormatSectionTitle("Bank Details"));

                            sectionSubtotal = 0;
                            budget[0] += build.BankUsage;
                            sectionSubtotal -= build.BankUsage;

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            switch (LoadedExportOptions.BudgetFormat)
                            {
                                case 0:
                                    budgetLastHalf = $"{leftBracket}+{build.BankUsage}{budgetSeparator}{budget[0]}{currencyAbbreviation}{rightBracket}";
                                    break;
                                case 1:
                                    budgetLastHalf = $"{leftBracket}+{build.BankUsage}{budgetSeparator}{sectionSubtotal}{currencyAbbreviation}{rightBracket}";
                                    break;
                                case 2:
                                    budgetLastHalf = $"{leftBracket}+{build.BankUsage}{currencyAbbreviation}{rightBracket}";
                                    break;
                                case 3:
                                    budgetLastHalf = $"{leftBracket}{budget[0]}{currencyAbbreviation}{rightBracket}";
                                    break;
                                case 4:
                                    budgetLastHalf = $"{leftBracket}{sectionSubtotal}{currencyAbbreviation}{rightBracket}";
                                    break;
                                default:
                                    break;
                            }

                            line = $"Used {build.BankUsage} CP from bank. {budgetLastHalf}";
                            output.Add(line);
                            FormatBlankLine(output);


                            budget[0] -= build.BankedPoints;
                            sectionSubtotal += build.BankedPoints;

                            switch (LoadedExportOptions.BudgetFormat)
                            {
                                case 0:
                                    budgetLastHalf = $"{leftBracket}{build.BankedPoints}{budgetSeparator}{budget[0]}{currencyAbbreviation}{rightBracket}";
                                    break;
                                case 1:
                                    budgetLastHalf = $"{leftBracket}{build.BankedPoints}{budgetSeparator}{sectionSubtotal}{currencyAbbreviation}{rightBracket}";
                                    break;
                                case 2:
                                    budgetLastHalf = $"{leftBracket}{build.BankedPoints}{currencyAbbreviation}{rightBracket}";
                                    break;
                                case 3:
                                    budgetLastHalf = $"{leftBracket}{budget[0]}{currencyAbbreviation}{rightBracket}";
                                    break;
                                case 4:
                                    budgetLastHalf = $"{leftBracket}{sectionSubtotal}{currencyAbbreviation}{rightBracket}";
                                    break;
                                default:
                                    break;
                            }

                            line = $"Invested {build.BankedPoints} CP into bank. {budgetLastHalf}";
                            output.Add(line);
                        }
                        else
                        {
                            budget[0] += build.BankUsage;
                            budget[0] -= build.BankedPoints;
                        }
                        break;
                    case "Point Summary":
                        if (section.Enabled)
                        {
                            output.Add(FormatSectionTitle("Point Summary"));

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            int drawbackSum = 0;
                            int scenarioSum = 0;

                            foreach (Drawback drawback in build.DrawbackSelection)
                            {
                                drawbackSum += drawback.Value;
                            }
                            foreach (Drawback scenario in build.ScenarioSelection)
                            {
                                scenarioSum += scenario.Value;
                            }

                            if (characterIndex > 0)
                            {
                                Dictionary<string, int> additionalValues = new()
                                {
                                    {"Bank Usage", build.BankUsage },
                                    {"Banked Points", -build.BankedPoints },
                                    {"Universal Drawbacks", drawbackSupplementCP },
                                    {"Jump Drawbacks", drawbackSum },
                                    {"Jump Scenarios", scenarioSum }
                                };

                                output.Add(FormatPointSummary(build.PointStipend[0], additionalValues, currencyAbbreviation));
                            }
                            else
                            {
                                Dictionary<string, int> additionalValues = new()
                                {
                                    {"Point Stipend", build.PointStipend[0] },
                                    {"Bank Usage", build.BankUsage },
                                    {"Banked Points", -build.BankedPoints },
                                    {"Universal Drawbacks", drawbackSupplementCP },
                                    {"Jump Drawbacks", drawbackSum },
                                    {"Jump Scenarios", scenarioSum }
                                };

                                output.Add(FormatPointSummary(jump.Currencies[0].CurrencyBudget, additionalValues, currencyAbbreviation));
                            }
                        }
                        break;
                    case "Jump Details":
                        if (section.Enabled)
                        {
                            output.Add(FormatSectionTitle("Jump Details"));

                            FormatDocumentDetails(jump, output);
                        }
                        break;
                    case "Jump Duration":
                        if (section.Enabled)
                        {
                            output.Add(FormatSectionTitle("Jump Duration"));

                            output.Add($"Years: {jump.DurationYears}");
                            output.Add($"Months: {jump.DurationMonths}");
                            output.Add($"Days: {jump.DurationDays}");
                        }
                        break;
                    case "Origin":
                        if (section.Enabled)
                        {
                            output.Add(FormatSectionTitle("Origin"));

                            sectionSubtotal = 0;
                            budget[0] -= jump.OriginDetails[build.OriginIndex].Cost;
                            sectionSubtotal += jump.OriginDetails[build.OriginIndex].Cost;

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            budgetLastHalf = FormatBudgetLastHalf(jump.OriginDetails[build.OriginIndex].Cost, budget[0], sectionSubtotal, currencyAbbreviation);

                            if (jump.OriginDetails[build.OriginIndex].Cost > 0)
                            {
                                line = $"{jump.OriginDetails[build.OriginIndex].Name} {budgetLastHalf}";
                            }
                            else
                            {
                                line = jump.OriginDetails[build.OriginIndex].Name;
                            }

                            switch (LoadedExportOptions.ExportMode)
                            {
                                case "Generic":
                                    line = "Origin: " + line;
                                    break;
                                case "BBCode":
                                    line = "Origin: [B]" + line + "[/B]";
                                    break;
                                case "Markdown":
                                    line = "Origin: **" + line + "**";
                                    break;
                                default:
                                    break;
                            }


                            output.Add(line);

                            switch (LoadedExportOptions.ExportMode)
                            {
                                case "Generic":
                                    if (LoadedExportOptions.GenericFormattingOptions[4].Enabled)
                                    {
                                        output.Add(jump.OriginDetails[build.OriginIndex].Description);
                                    }
                                    break;
                                case "BBCode":
                                    if (LoadedExportOptions.BBCodeFormattingOptions[12].Enabled)
                                    {
                                        output.Add(jump.OriginDetails[build.OriginIndex].Description);
                                    }
                                    break;
                                case "Markdown":
                                    if (LoadedExportOptions.MarkdownFormattingOptions[10].Enabled)
                                    {
                                        output.Add(jump.OriginDetails[build.OriginIndex].Description);
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                        else
                        {
                            budget[0] -= jump.OriginDetails[build.OriginIndex].Cost;
                        }
                        break;
                    case "Age":
                        if (section.Enabled)
                        {
                            sectionSubtotal = 0;
                            budget[0] -= build.AgeCost;
                            sectionSubtotal += build.AgeCost;

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            budgetLastHalf = FormatBudgetLastHalf(build.AgeCost, budget[0], sectionSubtotal, currencyAbbreviation);

                            if (build.AgeCost > 0)
                            {
                                line = $"{build.Age} {budgetLastHalf}";
                            }
                            else
                            {
                                line = $"{build.Age}";
                            }

                            switch (LoadedExportOptions.ExportMode)
                            {
                                case "Generic":
                                    line = "Age: " + line;
                                    break;
                                case "BBCode":
                                    line = "Age: [B]" + line + "[/B]";
                                    break;
                                case "Markdown":
                                    line = "Age: **" + line + "**";
                                    break;
                                default:
                                    break;
                            }

                            output.Add(line);
                        }
                        else
                        {
                            budget[0] -= build.AgeCost;
                        }
                        break;
                    case "Gender":
                        if (section.Enabled)
                        {
                            sectionSubtotal = 0;
                            budget[0] -= build.GenderCost;
                            sectionSubtotal += build.GenderCost;

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            budgetLastHalf = FormatBudgetLastHalf(build.GenderCost, budget[0], sectionSubtotal, currencyAbbreviation);

                            if (build.GenderCost > 0)
                            {
                                line = $"{build.Gender} {budgetLastHalf}";
                            }
                            else
                            {
                                line = build.Gender;
                            }

                            switch (LoadedExportOptions.ExportMode)
                            {
                                case "Generic":
                                    line = "Gender: " + line;
                                    break;
                                case "BBCode":
                                    line = "Gender: [B]" + line + "[/B]";
                                    break;
                                case "Markdown":
                                    line = "Gender: **" + line + "**";
                                    break;
                                default:
                                    break;
                            }

                            output.Add(line);
                        }
                        else
                        {
                            budget[0] -= build.GenderCost;
                        }
                        break;
                    case "Location":
                        if (section.Enabled)
                        {
                            sectionSubtotal = 0;
                            budget[0] -= build.Location.Cost;
                            sectionSubtotal += build.Location.Cost;

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            budgetLastHalf = FormatBudgetLastHalf(build.Location.Cost, budget[0], sectionSubtotal, currencyAbbreviation);

                            if (build.Location.Cost > 0)
                            {
                                line = $"{build.Location.Name} {budgetLastHalf}";
                            }
                            else
                            {
                                line = build.Location.Name;
                            }

                            switch (LoadedExportOptions.ExportMode)
                            {
                                case "Generic":
                                    line = "Location: " + line;
                                    break;
                                case "BBCode":
                                    line = "Location: [B]" + line + "[/B]";
                                    break;
                                case "Markdown":
                                    line = "Location: **" + line + "**";
                                    break;
                                default:
                                    break;
                            }

                            output.Add(line);

                            switch (LoadedExportOptions.ExportMode)
                            {
                                case "Generic":
                                    if (LoadedExportOptions.GenericFormattingOptions[4].Enabled)
                                    {
                                        output.Add(build.Location.Description);
                                    }
                                    break;
                                case "BBCode":
                                    if (LoadedExportOptions.BBCodeFormattingOptions[12].Enabled)
                                    {
                                        output.Add(build.Location.Description);
                                    }
                                    break;
                                case "Markdown":
                                    if (LoadedExportOptions.MarkdownFormattingOptions[10].Enabled)
                                    {
                                        output.Add(build.Location.Description);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            budget[0] -= build.Location.Cost;
                        }
                        break;
                    case "Species":
                        if (section.Enabled)
                        {
                            sectionSubtotal = 0;
                            budget[0] -= build.Species.Cost;
                            sectionSubtotal += build.Species.Cost;

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            budgetLastHalf = FormatBudgetLastHalf(build.Species.Cost, budget[0], sectionSubtotal, currencyAbbreviation);

                            if (build.Species.Cost > 0)
                            {
                                line = $"{build.Species.Name} {budgetLastHalf}";
                            }
                            else
                            {
                                line = build.Species.Name;
                            }

                            switch (LoadedExportOptions.ExportMode)
                            {
                                case "Generic":
                                    line = "Species: " + line;
                                    break;
                                case "BBCode":
                                    line = "Species: [B]" + line + "[/B]";
                                    break;
                                case "Markdown":
                                    line = "Species: **" + line + "**";
                                    break;
                                default:
                                    break;
                            }

                            output.Add(line);

                            switch (LoadedExportOptions.ExportMode)
                            {
                                case "Generic":
                                    if (LoadedExportOptions.GenericFormattingOptions[4].Enabled)
                                    {
                                        output.Add(build.Species.Description);
                                    }
                                    break;
                                case "BBCode":
                                    if (LoadedExportOptions.BBCodeFormattingOptions[12].Enabled)
                                    {
                                        output.Add(build.Species.Description);
                                    }
                                    break;
                                case "Markdown":
                                    if (LoadedExportOptions.MarkdownFormattingOptions[10].Enabled)
                                    {
                                        output.Add(build.Species.Description);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            budget[0] -= build.Species.Cost;
                        }
                        break;
                    case "Misc Origin Details":
                        if (section.Enabled)
                        {
                            output.Add(FormatSectionTitle("Origin"));

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            sectionSubtotal = 0;

                            foreach (OriginDetail detail in build.MiscOriginDetails)
                            {
                                budget[0] -= detail.Cost;
                                sectionSubtotal += detail.Cost;

                                budgetLastHalf = FormatBudgetLastHalf(detail.Cost, budget[0], sectionSubtotal, currencyAbbreviation);

                                if (build.Species.Cost > 0)
                                {
                                    line = $"{detail.Name} {budgetLastHalf}";
                                }
                                else
                                {
                                    line = detail.Name;
                                }

                                switch (LoadedExportOptions.ExportMode)
                                {
                                    case "Generic":
                                        line = $"{detail.Category}: " + line;
                                        break;
                                    case "BBCode":
                                        line = $"{detail.Category}: [B]" + line + "[/B]";
                                        break;
                                    case "Markdown":
                                        line = $"{detail.Category}: **" + line + "**";
                                        break;
                                    default:
                                        break;
                                }

                                output.Add(line);

                                switch (LoadedExportOptions.ExportMode)
                                {
                                    case "Generic":
                                        if (LoadedExportOptions.GenericFormattingOptions[4].Enabled)
                                        {
                                            output.Add(detail.Description);
                                        }
                                        break;
                                    case "BBCode":
                                        if (LoadedExportOptions.BBCodeFormattingOptions[12].Enabled)
                                        {
                                            output.Add(detail.Description);
                                        }
                                        break;
                                    case "Markdown":
                                        if (LoadedExportOptions.MarkdownFormattingOptions[10].Enabled)
                                        {
                                            output.Add(detail.Description);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            foreach (OriginDetail detail in build.MiscOriginDetails)
                            {
                                budget[0] -= detail.Cost;
                            }
                        }
                        break;
                    case "Perks":
                        if (section.Enabled)
                        {
                            foreach (PurchaseType type in jump.PurchaseTypes)
                            {
                                sectionSubtotal = 0;

                                currencyAbbreviation = FormatCurrencyAbbreviation(jump.Currencies[type.CurrencyIndex].CurrencyAbbreviation);

                                if (!type.IsItemType)
                                {
                                    output.Add(FormatSectionTitle(type.Type));

                                    foreach (Purchase purchase in build.Purchase)
                                    {
                                        if (purchase.TypeIndex == jump.PurchaseTypes.IndexOf(type))
                                        {
                                            budget[type.CurrencyIndex] -= purchase.DisplayCost;
                                            sectionSubtotal += purchase.DisplayCost;

                                            budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget[type.CurrencyIndex], sectionSubtotal, currencyAbbreviation);

                                            FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (PurchaseType type in jump.PurchaseTypes)
                            {
                                if (!type.IsItemType)
                                {
                                    foreach (Purchase purchase in build.Purchase)
                                    {
                                        if (purchase.TypeIndex == jump.PurchaseTypes.IndexOf(type))
                                        {
                                            budget[type.CurrencyIndex] -= purchase.DisplayCost;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "Items":
                        if (section.Enabled)
                        {
                            foreach (PurchaseType type in jump.PurchaseTypes)
                            {
                                sectionSubtotal = 0;

                                currencyAbbreviation = FormatCurrencyAbbreviation(jump.Currencies[type.CurrencyIndex].CurrencyAbbreviation);

                                if (type.IsItemType)
                                {
                                    line = type.Type;

                                    if (type.Type == "Items")
                                    {
                                        budget[0] += build.ItemStipend;

                                        if (build.ItemStipend > 0)
                                        {
                                            line += $" Stipend: {leftBracket}+{build.ItemStipend}{budgetSeparator}{budget[type.CurrencyIndex]}" +
                                                    $"{currencyAbbreviation}{rightBracket}";
                                        }

                                        budget[0] += drawbackSupplementItemCP;

                                        if (drawbackSupplementItemCP > 0)
                                        {
                                            line += $" Universal Drawbacks: {leftBracket}+{drawbackSupplementItemCP}{budgetSeparator}{budget[type.CurrencyIndex]}" +
                                                    $"{currencyAbbreviation}{rightBracket}";
                                        }
                                    }

                                    output.Add(FormatSectionTitle(line));

                                    foreach (Purchase purchase in build.Purchase)
                                    {
                                        if (purchase.TypeIndex == jump.PurchaseTypes.IndexOf(type))
                                        {
                                            budget[type.CurrencyIndex] -= purchase.DisplayCost;
                                            sectionSubtotal += purchase.DisplayCost;

                                            budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget[type.CurrencyIndex], sectionSubtotal, currencyAbbreviation);

                                            FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (PurchaseType type in jump.PurchaseTypes)
                            {
                                if (type.IsItemType)
                                {
                                    if (type.Type == "Items")
                                    {
                                        budget[0] += build.ItemStipend;
                                    }
                                    foreach (Purchase purchase in build.Purchase)
                                    {
                                        if (purchase.TypeIndex == jump.PurchaseTypes.IndexOf(type))
                                        {
                                            budget[type.CurrencyIndex] -= purchase.DisplayCost;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "Import Options":
                        if (section.Enabled)
                        {
                            sectionSubtotal = 0;

                            output.Add(FormatSectionTitle("Companion Imports"));

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            foreach (CompanionPurchase import in build.CompanionPurchase)
                            {
                                if (import.DisplayCost > 0)
                                {
                                    budget[0] -= import.DisplayCost;
                                    sectionSubtotal += import.DisplayCost;

                                    budgetLastHalf = FormatBudgetLastHalf(import.DisplayCost, budget[0], sectionSubtotal, currencyAbbreviation);

                                    FormatPricedDataLine(import.Name, import.Description, budgetLastHalf, output, import.DisplayCost);
                                }

                                output.Add("Imported Companions");

                                foreach (CompanionImportDetailClass companion in import.CompanionImportDetails)
                                {
                                    if (companion.CompanionSelected)
                                    {
                                        line = $"Imported {companion.CompanionName} (+{companion.CompanionOptionValue}{currencyAbbreviation})";
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (CompanionPurchase import in build.CompanionPurchase)
                            {
                                if (import.DisplayCost > 0)
                                {
                                    budget[0] -= import.DisplayCost;
                                }
                            }
                        }
                        break;
                    case "Drawbacks":
                        if (section.Enabled)
                        {
                            sectionSubtotal = 0;

                            output.Add(FormatSectionTitle("Drawbacks"));

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            foreach (Drawback drawback in build.DrawbackSelection)
                            {
                                budget[0] += drawback.Value;
                                sectionSubtotal += drawback.Value;

                                budgetLastHalf = FormatBudgetLastHalf(drawback.Value, budget[0], sectionSubtotal, currencyAbbreviation);

                                FormatPricedDataLine(drawback.Name, drawback.Description, budgetLastHalf, output, drawback.Value, drawback.Reward);
                            }
                        }
                        else
                        {
                            foreach (Drawback drawback in build.DrawbackSelection)
                            {
                                budget[0] += drawback.Value;
                            }
                        }
                        break;
                    case "Scenarios":
                        if (section.Enabled)
                        {
                            sectionSubtotal = 0;

                            output.Add(FormatSectionTitle("Scenarios"));

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            foreach (Drawback scenario in build.ScenarioSelection)
                            {
                                budget[0] += scenario.Value;
                                sectionSubtotal += scenario.Value;

                                budgetLastHalf = FormatBudgetLastHalf(scenario.Value, budget[0], sectionSubtotal, currencyAbbreviation);

                                FormatPricedDataLine(scenario.Name, scenario.Description, budgetLastHalf, output, scenario.Value, scenario.Reward);
                            }
                        }
                        else
                        {
                            foreach (Drawback scenario in build.ScenarioSelection)
                            {
                                budget[0] += scenario.Value;
                            }
                        }
                        break;
                    case "Supplements":
                        if (section.Enabled)
                        {
                            sectionSubtotal = 0;

                            output.Add(FormatSectionTitle("Supplement Investment"));

                            currencyAbbreviation = FormatCurrencyAbbreviation("CP");

                            switch (LoadedOptions.BodyModSetting)
                            {
                                case Options.BodyModSupplements.Generic:
                                    if (LoadedSave.GenericBodyMod.InvestmentAllowed)
                                    {
                                        budget[0] -= build.BodyModInvestment;
                                        sectionSubtotal += build.BodyModInvestment;

                                        budgetLastHalf = FormatBudgetLastHalf(build.BodyModInvestment, budget[0], sectionSubtotal, currencyAbbreviation);

                                        if (build.BodyModInvestment > 0)
                                        {
                                            line = $"Body Mod Investment {budgetLastHalf}";
                                        }

                                        output.Add(line);
                                    }
                                    break;
                                case Options.BodyModSupplements.SBBodyMod:
                                    if (LoadedSave.SBBodyMod.InvestmentAllowed)
                                    {
                                        budget[0] -= build.BodyModInvestment;
                                        sectionSubtotal += build.BodyModInvestment;

                                        budgetLastHalf = FormatBudgetLastHalf(build.BodyModInvestment, budget[0], sectionSubtotal, currencyAbbreviation);

                                        if (build.BodyModInvestment > 0)
                                        {
                                            line = $"SB Body Mod Investment {budgetLastHalf}";
                                        }

                                        output.Add(line);
                                    }
                                    break;
                                case Options.BodyModSupplements.EssentialBodyMod:
                                    if (LoadedSave.EssentialBodyMod.InvestmentAllowed)
                                    {
                                        budget[0] -= build.BodyModInvestment;
                                        sectionSubtotal += build.BodyModInvestment;

                                        budgetLastHalf = FormatBudgetLastHalf(build.BodyModInvestment, budget[0], sectionSubtotal, currencyAbbreviation);

                                        if (build.BodyModInvestment > 0)
                                        {
                                            line = $"Essential Body Mod Investment {budgetLastHalf}";
                                        }

                                        output.Add(line);
                                    }
                                    break;
                                default:
                                    break;
                            }

                            switch (LoadedOptions.CosmicWarehouseSetting)
                            {
                                case Options.CosmicWarehouseSupplements.Generic:
                                    if (LoadedSave.GenericWarehouse.InvestmentAllowed)
                                    {
                                        budget[0] -= build.WarehouseInvestment;
                                        sectionSubtotal += build.WarehouseInvestment;

                                        budgetLastHalf = FormatBudgetLastHalf(build.WarehouseInvestment, budget[0], sectionSubtotal, currencyAbbreviation);

                                        if (build.WarehouseInvestment > 0)
                                        {
                                            line = $"Warehouse Investment {budgetLastHalf}";
                                        }

                                        output.Add(line);
                                    }
                                    break;
                                case Options.CosmicWarehouseSupplements.PersonalReality:
                                    if (LoadedSave.PersonalReality.InvestmentAllowed)
                                    {
                                        budget[0] -= build.WarehouseInvestment;
                                        sectionSubtotal += build.WarehouseInvestment;

                                        budgetLastHalf = FormatBudgetLastHalf(build.WarehouseInvestment, budget[0], sectionSubtotal, currencyAbbreviation);

                                        if (build.WarehouseInvestment > 0)
                                        {
                                            line = $"Personal Reality Investment {budgetLastHalf}";
                                        }

                                        output.Add(line);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (LoadedOptions.BodyModSetting)
                            {
                                case Options.BodyModSupplements.Generic:
                                    if (LoadedSave.GenericBodyMod.InvestmentAllowed)
                                    {
                                        budget[0] -= build.BodyModInvestment;
                                    }
                                    break;
                                case Options.BodyModSupplements.SBBodyMod:
                                    if (LoadedSave.SBBodyMod.InvestmentAllowed)
                                    {
                                        budget[0] -= build.BodyModInvestment;
                                    }
                                    break;
                                case Options.BodyModSupplements.EssentialBodyMod:
                                    if (LoadedSave.EssentialBodyMod.InvestmentAllowed)
                                    {
                                        budget[0] -= build.BodyModInvestment;
                                    }
                                    break;
                                default:
                                    break;
                            }

                            switch (LoadedOptions.CosmicWarehouseSetting)
                            {
                                case Options.CosmicWarehouseSupplements.Generic:
                                    if (LoadedSave.GenericWarehouse.InvestmentAllowed)
                                    {
                                        budget[0] -= build.WarehouseInvestment;
                                    }
                                    break;
                                case Options.CosmicWarehouseSupplements.PersonalReality:
                                    if (LoadedSave.PersonalReality.InvestmentAllowed)
                                    {
                                        budget[0] -= build.WarehouseInvestment;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }

                FormatSectionEnding(output, section.Enabled, sectionSeparator);
            }

            FormatClosingLines(output);

            TxtAccess.WriteExportText($"{jump.Name} {CharacterList[characterIndex].Name} Build", "Builds", output);
        }

        private void ExportJumpData(Jump jump)
        {
            char sectionSeparator = LoadedExportOptions.SectionSeparator;

            List<string> output = new();

            if (jump.IsGauntlet)
            {
                if (jump.Name.EndsWith("Gauntlet"))
                {
                    FormatOpeningLines(output, $"{jump.Name}");
                }
                else
                {
                    FormatOpeningLines(output, $"{jump.Name} Gauntlet");
                }
            }
            else
            {
                if (jump.Name.EndsWith("Jump"))
                {
                    FormatOpeningLines(output, $"{jump.Name}");
                }
                else
                {
                    FormatOpeningLines(output, $"{jump.Name} Jump");
                }
            }

            FormatDocumentDetails(jump, output);

            FormatBlankLine(output);

            output.Add("Jump is #" + jump.JumpNumber + " in Chain");

            FormatBlankLine(output);

            FormatSectionEnding(output, true, sectionSeparator);

            output.Add(FormatSectionTitle("Jump Duration"));
            output.Add("Years: " + jump.DurationYears);
            output.Add("Months: " + jump.DurationMonths);
            output.Add("Days: " + jump.DurationDays);

            FormatBlankLine(output);

            FormatClosingLines(output);

            TxtAccess.WriteExportText($"{CharacterList[0].Name} - {jump.Name}", "Jump Details", output);
        }

        private void ExportProfile(Character character, int characterIndex)
        {
            Dictionary<string, List<Purchase>> perks = new();
            Dictionary<string, List<Purchase>> items = new();

            perks.Add("Physical Perks", new());
            perks.Add("Mental Perks", new());
            perks.Add("Social Perks", new());
            perks.Add("Stealth Perks", new());
            perks.Add("Magical Perks", new());
            perks.Add("Spiritual Perks", new());
            perks.Add("Technological Perks", new());
            perks.Add("Crafting Perks", new());
            perks.Add("Blacksmithing Perks", new());
            perks.Add("Meta Perks", new());
            perks.Add("Other Perks", new());

            items.Add("Weapons", new());
            items.Add("Armor", new());
            items.Add("Accessories", new());
            items.Add("Clothing", new());
            items.Add("Misc. Equipment", new());
            items.Add("Tools", new());
            items.Add("Materials", new());
            items.Add("Food", new());
            items.Add("Media", new());
            items.Add("Wealth", new());
            items.Add("Vehicles", new());
            items.Add("Properties", new());
            items.Add("Businesses", new());
            items.Add("Creatures", new());
            items.Add("Other Item", new());

            foreach (Jump jump in JumpList)
            {
                if (jump.Build.Count > characterIndex && jump.Build[characterIndex] != null)
                {
                    foreach (Purchase purchase in jump.Build[characterIndex].Purchase)
                    {
                        if (jump.PurchaseTypes[purchase.TypeIndex].IsItemType)
                        {
                            switch (purchase.Category)
                            {
                                case "Weapons":
                                    items["Weapons"].Add(purchase);
                                    break;
                                case "Armor":
                                    items["Armor"].Add(purchase);
                                    break;
                                case "Accessories":
                                    items["Accessories"].Add(purchase);
                                    break;
                                case "Clothing":
                                    items["Clothing"].Add(purchase);
                                    break;
                                case "Misc. Equipment":
                                    items["Misc. Equipment"].Add(purchase);
                                    break;
                                case "Tools":
                                    items["Tools"].Add(purchase);
                                    break;
                                case "Materials":
                                    items["Materials"].Add(purchase);
                                    break;
                                case "Food":
                                    items["Food"].Add(purchase);
                                    break;
                                case "Media":
                                    items["Media"].Add(purchase);
                                    break;
                                case "Wealth":
                                    items["Wealth"].Add(purchase);
                                    break;
                                case "Vehicles":
                                    items["Vehicles"].Add(purchase);
                                    break;
                                case "Properties":
                                    items["Properties"].Add(purchase);
                                    break;
                                case "Businesses":
                                    items["Businesses"].Add(purchase);
                                    break;
                                case "Creatures":
                                    items["Creatures"].Add(purchase);
                                    break;
                                case "Other Item":
                                    items["Other Item"].Add(purchase);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (purchase.Category)
                            {
                                case "Physical":
                                    perks["Physical Perks"].Add(purchase);
                                    break;
                                case "Mental":
                                    perks["Mental Perks"].Add(purchase);
                                    break;
                                case "Social":
                                    perks["Social Perks"].Add(purchase);
                                    break;
                                case "Stealth":
                                    perks["Stealth Perks"].Add(purchase);
                                    break;
                                case "Magical":
                                    perks["Magical Perks"].Add(purchase);
                                    break;
                                case "Spiritual":
                                    perks["Spiritual Perks"].Add(purchase);
                                    break;
                                case "Technological":
                                    perks["Technological Perks"].Add(purchase);
                                    break;
                                case "Crafting":
                                    perks["Crafting Perks"].Add(purchase);
                                    break;
                                case "Blacksmithing":
                                    perks["Blacksmithing Perks"].Add(purchase);
                                    break;
                                case "Meta":
                                    perks["Meta Perks"].Add(purchase);
                                    break;
                                case "Other Perk":
                                    perks["Other Perks"].Add(purchase);
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }
            }

            char sectionSeparator = LoadedExportOptions.SectionSeparator;

            List<string> output = new();
            string line = "";

            FormatOpeningLines(output, $"{character.Name} Profile");

            foreach (ExportFormatToggle section in LoadedExportOptions.ProfileSectionList)
            {
                if (section.Enabled)
                {
                    switch (section.Name)
                    {
                        case "Profile":
                            output.Add(FormatSectionTitle("Profile Details"));

                            FormatBlankLine(output);

                            foreach (ExportFormatToggle subsection in LoadedExportOptions.ProfileSubsectionList)
                            {
                                if (subsection.Enabled)
                                {
                                    switch (subsection.Name)
                                    {
                                        case "Biography":
                                            output.Add(FormatSectionTitle("Biography"));

                                            FormatBlankLine(output);

                                            if (character.Name != "")
                                            {
                                                output.Add($"Name: {character.Name}");

                                                FormatBlankLine(output);
                                            }

                                            if (character.Alias != "")
                                            {
                                                output.Add($"Alias: {character.Alias}");

                                                FormatBlankLine(output);
                                            }

                                            if (characterIndex > 0 && character.FirstJump >= 0)
                                            {
                                                output.Add($"Joined on Jump #{character.FirstJump}");

                                                FormatBlankLine(output);
                                            }

                                            if (character.Age > 0)
                                            {
                                                output.Add($"Original Age: {character.Age}");

                                                FormatBlankLine(output);
                                            }

                                            if (character.TrueAge > 0)
                                            {
                                                output.Add($"Current Age: {character.TrueAge}");

                                                FormatBlankLine(output);
                                            }

                                            if (character.Gender != "")
                                            {
                                                output.Add($"Gender: {character.Gender}");

                                                FormatBlankLine(output);
                                            }

                                            if (character.Homeworld != "")
                                            {
                                                output.Add($"Original homeworld/setting: {character.Homeworld}");

                                                FormatBlankLine(output);
                                            }
                                            break;
                                        case "Physical Characteristics":
                                            output.Add(FormatSectionTitle("Physical Characteristics"));

                                            FormatBlankLine(output);

                                            if (character.Species != "")
                                            {
                                                output.Add($"Species: {character.Species}");

                                                FormatBlankLine(output);
                                            }

                                            if (character.Race != "")
                                            {
                                                output.Add($"Race: {character.Race}");

                                                FormatBlankLine(output);
                                            }

                                            switch (AppSettings.HeightFormat)
                                            {
                                                case AppSettingsModel.HeightFormats.FeetInches:
                                                    if (character.HeightInches > 0)
                                                    {
                                                        output.Add($"Height (Inches): {character.HeightInches}");

                                                        FormatBlankLine(output);
                                                    }
                                                    break;
                                                case AppSettingsModel.HeightFormats.Feet:
                                                    if (character.HeightFeet > 0)
                                                    {
                                                        output.Add($"Height (Feet): {character.HeightFeet}");

                                                        FormatBlankLine(output);
                                                    }
                                                    break;
                                                case AppSettingsModel.HeightFormats.Meters:
                                                    if (character.HeightMeters > 0)
                                                    {
                                                        output.Add($"Height (Meters): {character.HeightMeters}");

                                                        FormatBlankLine(output);
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }

                                            switch (AppSettings.WeightFormat)
                                            {
                                                case AppSettingsModel.WeightFormats.Pounds:
                                                    if (character.WeightPounds > 0)
                                                    {
                                                        output.Add($"Weight (Pounds): {character.WeightPounds}");

                                                        FormatBlankLine(output);
                                                    }
                                                    break;
                                                case AppSettingsModel.WeightFormats.Kilograms:
                                                    if (character.WeightKilograms > 0)
                                                    {
                                                        output.Add($"Weight (Kilograms): {character.WeightKilograms}");

                                                        FormatBlankLine(output);
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                            break;
                                        case "Personality":
                                            if (character.Personality != "")
                                            {
                                                output.Add(FormatSectionTitle("Personality"));

                                                FormatBlankLine(output);

                                                output.Add(character.Personality);
                                            }
                                            break;
                                        case "Physical Description":
                                            if (character.PhysicalDescription != "")
                                            {
                                                output.Add(FormatSectionTitle("Physical Description"));

                                                FormatBlankLine(output);

                                                output.Add(character.PhysicalDescription);
                                            }
                                            break;
                                        case "Traits":
                                            List<string> likes = new();
                                            List<string> dislikes = new();
                                            List<string> hobbies = new();
                                            List<string> quirks = new();
                                            List<string> goals = new();

                                            output.Add(FormatSectionTitle("Traits"));

                                            FormatBlankLine(output);

                                            foreach (Trait row in character.TraitRow)
                                            {
                                                if (row.Like != "")
                                                {
                                                    likes.Add(row.Like);
                                                }
                                                if (row.Dislike != "")
                                                {
                                                    dislikes.Add(row.Dislike);
                                                }
                                                if (row.Hobby != "")
                                                {
                                                    hobbies.Add(row.Hobby);
                                                }
                                                if (row.Quirk != "")
                                                {
                                                    quirks.Add(row.Quirk);
                                                }
                                                if (row.Goal != "")
                                                {
                                                    goals.Add(row.Goal);
                                                }
                                            }

                                            if (likes.Any())
                                            {
                                                output.Add(FormatSectionTitle("Likes"));

                                                FormatBlankLine(output);

                                                foreach (string like in likes)
                                                {
                                                    output.Add(like);
                                                }
                                            }

                                            if (dislikes.Any())
                                            {
                                                output.Add(FormatSectionTitle("Dislikes"));

                                                FormatBlankLine(output);

                                                foreach (string dislike in dislikes)
                                                {
                                                    output.Add(dislike);
                                                }
                                            }

                                            if (hobbies.Any())
                                            {
                                                output.Add(FormatSectionTitle("Hobbies"));

                                                FormatBlankLine(output);

                                                foreach (string hobby in hobbies)
                                                {
                                                    output.Add(hobby);
                                                }
                                            }

                                            if (quirks.Any())
                                            {
                                                output.Add(FormatSectionTitle("Quirks"));

                                                FormatBlankLine(output);

                                                foreach (string quirk in quirks)
                                                {
                                                    output.Add(quirk);
                                                }
                                            }

                                            if (goals.Any())
                                            {
                                                output.Add(FormatSectionTitle("Goals"));

                                                FormatBlankLine(output);

                                                foreach (string goal in goals)
                                                {
                                                    output.Add(goal);
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            break;
                        case "Alt Forms":
                            if (character.AltForms.Any())
                            {
                                output.Add(FormatSectionTitle("Alt-Form Details"));

                                FormatBlankLine(output);

                                foreach (AltForm form in character.AltForms)
                                {
                                    List<string> strengths = new();
                                    List<string> weaknesses = new();

                                    if (LoadedExportOptions.ExportMode == "BBCode" && LoadedExportOptions.BBCodeFormattingOptions[4].Enabled)
                                    {
                                        if (form.AltFormName != "")
                                        {
                                            line = $"[SPOILER=\"{form.AltFormName}\"]";
                                        }
                                        else if (form.AltFormSpecies != "")
                                        {
                                            line = $"[SPOILER=\"{form.AltFormSpecies}\"]";
                                        }

                                        output.Add(line);

                                        FormatBlankLine(output);
                                    }
                                    else
                                    {
                                        if (form.AltFormName != "")
                                        {
                                            output.Add(FormatSectionTitle(form.AltFormName));
                                        }
                                        else if (form.AltFormSpecies != "")
                                        {
                                            output.Add(FormatSectionTitle(form.AltFormSpecies));
                                        }

                                        FormatBlankLine(output);
                                    }

                                    if (form.AltFormName != "")
                                    {
                                        output.Add($"Alt-form Name: {form.AltFormName}");

                                        FormatBlankLine(output);
                                    }

                                    if (form.AltFormSpecies != "")
                                    {
                                        output.Add($"Species: {form.AltFormSpecies}");

                                        FormatBlankLine(output);
                                    }

                                    if (form.AltFormDescription != "")
                                    {
                                        output.Add($"Description: {form.AltFormDescription}");

                                        FormatBlankLine(output);
                                    }

                                    foreach (AltFormTraitModel row in form.StrengthWeaknessRow)
                                    {
                                        if (row.Strength != "")
                                        {
                                            strengths.Add(row.Strength);
                                        }
                                        if (row.Weakness != "")
                                        {
                                            weaknesses.Add(row.Weakness);
                                        }
                                    }

                                    if (strengths.Any())
                                    {
                                        output.Add("Strengths:");

                                        foreach (string strength in strengths)
                                        {
                                            output.Add(strength);
                                        }

                                        FormatBlankLine(output);
                                    }
                                    if (weaknesses.Any())
                                    {
                                        output.Add("Weaknesses:");

                                        foreach (string weakness in weaknesses)
                                        {
                                            output.Add(weakness);
                                        }

                                        FormatBlankLine(output);
                                    }

                                    if (LoadedExportOptions.ExportMode == "BBCode" && LoadedExportOptions.BBCodeFormattingOptions[4].Enabled)
                                    {
                                        output.Add("[/SPOILER]");
                                    }
                                }
                            }
                            break;
                        case "Perks":
                            output.Add(FormatSectionTitle("Perk Details"));

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Physical Perks"));

                            foreach (Purchase perk in perks["Physical Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Mental Perks"));

                            foreach (Purchase perk in perks["Mental Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Social Perks"));

                            foreach (Purchase perk in perks["Social Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Stealth Perks"));

                            foreach (Purchase perk in perks["Stealth Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Magical Perks"));

                            foreach (Purchase perk in perks["Magical Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Spiritual Perks"));

                            foreach (Purchase perk in perks["Spiritual Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Technological Perks"));

                            foreach (Purchase perk in perks["Technological Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Crafting Perks"));

                            foreach (Purchase perk in perks["Crafting Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Blacksmithing Perks"));

                            foreach (Purchase perk in perks["Blacksmithing Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Meta Perks"));

                            foreach (Purchase perk in perks["Meta Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Other Perks"));

                            foreach (Purchase perk in perks["Other Perks"])
                            {
                                FormatNonPricedDataLine(perk.Name, perk.Description, output, sourceJump: perk.SourceJump);

                                FormatBlankLine(output);
                            }
                            break;
                        case "Items":
                            output.Add(FormatSectionTitle("Item Details"));

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Weapons"));

                            foreach (Purchase item in items["Weapons"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Armor"));

                            foreach (Purchase item in items["Armor"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Accessories"));

                            foreach (Purchase item in items["Accessories"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Clothing"));

                            foreach (Purchase item in items["Clothing"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Misc. Equipment"));

                            foreach (Purchase item in items["Misc. Equipment"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Tools"));

                            foreach (Purchase item in items["Tools"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Materials"));

                            foreach (Purchase item in items["Materials"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Food"));

                            foreach (Purchase item in items["Food"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Media"));

                            foreach (Purchase item in items["Media"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Wealth"));

                            foreach (Purchase item in items["Wealth"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Vehicles"));

                            foreach (Purchase item in items["Vehicles"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Properties"));

                            foreach (Purchase item in items["Properties"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Businesses"));

                            foreach (Purchase item in items["Businesses"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Creatures"));

                            foreach (Purchase item in items["Creatures"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }

                            FormatBlankLine(output);

                            output.Add(FormatSectionTitle("Other Item"));

                            foreach (Purchase item in items["Other Item"])
                            {
                                FormatNonPricedDataLine(item.Name, item.Description, output, sourceJump: item.SourceJump);

                                FormatBlankLine(output);
                            }
                            break;
                        case "Attributes":
                            output.Add(FormatSectionTitle("Attributes"));

                            FormatBlankLine(output);

                            List<ProfileAttribute> physicalAttributes = new();
                            List<ProfileAttribute> mentalAttributes = new();
                            List<ProfileAttribute> supernaturalAttributes = new();

                            foreach (ProfileAttribute attribute in character.Attributes)
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalAttributes.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalAttributes.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalAttributes.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (physicalAttributes.Any())
                            {
                                output.Add(FormatSectionTitle("Physical"));

                                foreach (ProfileAttribute attribute in physicalAttributes)
                                {
                                    output.Add($"Name: {attribute.Name}");
                                    output.Add($"Rank: {attribute.DisplayRank}");

                                    FormatBlankLine(output);
                                }
                            }

                            if (mentalAttributes.Any())
                            {
                                output.Add(FormatSectionTitle("Mental"));

                                foreach (ProfileAttribute attribute in mentalAttributes)
                                {
                                    output.Add($"Name: {attribute.Name}");
                                    output.Add($"Rank: {attribute.DisplayRank}");

                                    FormatBlankLine(output);
                                }
                            }

                            if (supernaturalAttributes.Any())
                            {
                                output.Add(FormatSectionTitle("Supernatural"));

                                foreach (ProfileAttribute attribute in supernaturalAttributes)
                                {
                                    output.Add($"Name: {attribute.Name}");
                                    output.Add($"Rank: {attribute.DisplayRank}");

                                    FormatBlankLine(output);
                                }
                            }
                            break;
                        case "Skills":
                            output.Add(FormatSectionTitle("Skills"));

                            FormatBlankLine(output);

                            List<ProfileAttribute> physicalSkills = new();
                            List<ProfileAttribute> mentalSkills = new();
                            List<ProfileAttribute> socialSkills = new();
                            List<ProfileAttribute> technologicalSkills = new();
                            List<ProfileAttribute> supernaturalSkills = new();

                            foreach (ProfileAttribute skill in character.Skills)
                            {
                                switch (skill.Category)
                                {
                                    case "Physical":
                                        physicalSkills.Add(skill);
                                        break;
                                    case "Mental":
                                        mentalSkills.Add(skill);
                                        break;
                                    case "Social":
                                        socialSkills.Add(skill);
                                        break;
                                    case "Technological":
                                        technologicalSkills.Add(skill);
                                        break;
                                    case "Supernatural":
                                        supernaturalSkills.Add(skill);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (physicalSkills.Any())
                            {
                                output.Add(FormatSectionTitle("Physical"));

                                foreach (ProfileAttribute skill in physicalSkills)
                                {
                                    output.Add($"Name: {skill.Name}");
                                    output.Add($"Rank: {skill.DisplayRank}");

                                    FormatBlankLine(output);
                                }
                            }

                            if (mentalSkills.Any())
                            {
                                output.Add(FormatSectionTitle("Mental"));

                                foreach (ProfileAttribute skill in mentalSkills)
                                {
                                    output.Add($"Name: {skill.Name}");
                                    output.Add($"Rank: {skill.DisplayRank}");

                                    FormatBlankLine(output);
                                }
                            }

                            if (socialSkills.Any())
                            {
                                output.Add(FormatSectionTitle("Social"));

                                foreach (ProfileAttribute skill in socialSkills)
                                {
                                    output.Add($"Name: {skill.Name}");
                                    output.Add($"Rank: {skill.DisplayRank}");

                                    FormatBlankLine(output);
                                }
                            }

                            if (technologicalSkills.Any())
                            {
                                output.Add(FormatSectionTitle("Technological"));

                                foreach (ProfileAttribute skill in technologicalSkills)
                                {
                                    output.Add($"Name: {skill.Name}");
                                    output.Add($"Rank: {skill.DisplayRank}");

                                    FormatBlankLine(output);
                                }
                            }

                            if (supernaturalSkills.Any())
                            {
                                output.Add(FormatSectionTitle("Supernatural"));

                                foreach (ProfileAttribute skill in supernaturalSkills)
                                {
                                    output.Add($"Name: {skill.Name}");
                                    output.Add($"Rank: {skill.DisplayRank}");

                                    FormatBlankLine(output);
                                }
                            }
                            break;
                        case "Learning Rates":
                            output.Add(FormatSectionTitle("Learning Rates"));

                            FormatBlankLine(output);

                            foreach (Booster booster in character.Boosters)
                            {
                                output.Add(booster.BoosterName + ": " + booster.BoosterDescription);
                                output.Add($"Raw Rate: {booster.BoosterRaw}");
                                output.Add($"Modified Rate: {booster.BoosterMultiplier}");

                                FormatBlankLine(output);
                            }
                            break;
                        default:
                            break;
                    }

                    FormatSectionEnding(output, section.Enabled, sectionSeparator);
                }
            }

            FormatClosingLines(output);

            TxtAccess.WriteExportText($"{CharacterList[characterIndex].Name} Profile", "Profiles", output);
        }

        private void ExportWarehouse(Character character, Options.CosmicWarehouseSupplements loadedWarehouse)
        {
            DrawbackSupplementUniversal drawbackSupplement = new();

            switch (LoadedOptions.DrawbackSupplementSetting)
            {
                case Options.DrawbackSupplements.Generic:
                    drawbackSupplement = LoadedSave.GenericDrawbackSupplement;
                    break;
                case Options.DrawbackSupplements.UDS:
                    drawbackSupplement = LoadedSave.UniversalDrawbackSupplement;
                    break;
                case Options.DrawbackSupplements.UU:
                    drawbackSupplement = LoadedSave.UUSupplement;
                    break;
                default:
                    break;
            }

            char sectionSeparator = LoadedExportOptions.SectionSeparator;
            string budgetLastHalf;
            string currencyAbbreviation;

            int budget = 0;
            int sectionSubtotal;

            int drawbackSupplementWP;
            int supplementLimitationWP = 0;
            int jumpWP = 0;
            int investmentWP = 0;
            int patientJumperWP = 0;

            List<Purchase> warehouseAddons = new();

            List<string> output = new();
            string line;

            drawbackSupplementWP = drawbackSupplement.WPGained;

            switch (loadedWarehouse)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    if (LoadedSave.GenericWarehouse.IncrementalBudget > 0)
                    {
                        jumpWP = CalculateJumpWP(LoadedSave.GenericWarehouse);
                    }
                    else
                    {
                        jumpWP = 0;
                    }

                    if (LoadedSave.GenericWarehouse.InvestmentAllowed)
                    {
                        investmentWP = CalculateInvestmentWP(LoadedSave.GenericWarehouse);
                    }

                    supplementLimitationWP = CalculateLimitationWP(LoadedSave.GenericWarehouse);

                    budget = LoadedSave.GenericWarehouse.Budget;
                    budget += jumpWP;
                    budget += investmentWP;
                    budget += drawbackSupplementWP;
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    if (LoadedSave.PersonalReality.IncrementalBudget > 0)
                    {
                        jumpWP = CalculateJumpWP(LoadedSave.PersonalReality);
                    }
                    else
                    {
                        jumpWP = 0;
                    }

                    if (LoadedSave.PersonalReality.InvestmentAllowed)
                    {
                        investmentWP = CalculateInvestmentWP(LoadedSave.PersonalReality);
                    }

                    if (LoadedSave.PersonalReality.PatientJumper)
                    {
                        if (LoadedSave.PersonalReality.PatientJumperCountFirstJump)
                        {
                            int delay = LoadedSave.PersonalReality.SupplementDelay - 1;
                            if (delay < 0)
                            {
                                delay = 0;
                            }

                            patientJumperWP = delay * 100;
                        }
                        else
                        {
                            int delay = LoadedSave.PersonalReality.SupplementDelay - 2;
                            if (delay < 0)
                            {
                                delay = 0;
                            }

                            patientJumperWP = delay * 100;
                        }
                    }

                    supplementLimitationWP = CalculateLimitationWP(LoadedSave.PersonalReality);

                    budget = LoadedSave.PersonalReality.Budget;
                    budget += jumpWP;
                    budget += investmentWP;
                    budget += drawbackSupplementWP;
                    budget += patientJumperWP;
                    break;
                default:
                    break;
            }

            foreach (Jump jump in LoadedSave.JumpList)
            {
                foreach (JumpBuild build in jump.Build)
                {
                    foreach (Purchase purchase in build.Purchase)
                    {
                        if (purchase.Category == "Warehouse Addon")
                        {
                            warehouseAddons.Add(purchase);
                            purchase.SourceJump = jump.Name;
                            purchase.SourceCharacter = LoadedSave.CharacterList[jump.Build.IndexOf(build)].Name;
                        }
                    }
                }
            }

            FormatOpeningLines(output, $"{character.Name}'s Cosmic Warehouse");

            switch (loadedWarehouse)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    foreach (ExportFormatToggle section in LoadedExportOptions.GenericWarehouseSectionList)
                    {
                        switch (section.Name)
                        {
                            case "Point Summary":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Point Summary"));

                                    currencyAbbreviation = FormatCurrencyAbbreviation("WP");

                                    Dictionary<string, int> additionalValues = new()
                                    {
                                        {"Per-Jump WP gained", jumpWP },
                                        {"Investment WP", investmentWP },
                                        {"Universal Drawback WP", drawbackSupplementWP },
                                        {"Limitation WP", supplementLimitationWP }
                                    };

                                    output.Add(FormatPointSummary(LoadedSave.GenericWarehouse.Budget, additionalValues, currencyAbbreviation));
                                }
                                break;
                            case "Block Description":
                                if (section.Enabled)
                                {
                                    if (!LoadedSave.GenericWarehouse.PurchasesAllowed)
                                    {
                                        output.Add(FormatSectionTitle("Warehouse Supplement Description"));

                                        FormatBlankLine(output);

                                        output.Add(LoadedSave.GenericWarehouse.FullDescription);
                                    }
                                }
                                break;
                            case "Purchases":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Purchases"));
                                    FormatBlankLine(output);

                                    sectionSubtotal = 0;

                                    currencyAbbreviation = FormatCurrencyAbbreviation("WP");

                                    foreach (SupplementPurchase purchase in LoadedSave.GenericWarehouse.Purchases)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }
                                }
                                else
                                {
                                    foreach (SupplementPurchase purchase in LoadedSave.GenericWarehouse.Purchases)
                                    {
                                        budget -= purchase.DisplayCost;
                                    }
                                }
                                break;
                            case "Additions":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Warehouse Additions"));

                                    FormatBlankLine(output);

                                    foreach (Purchase purchase in warehouseAddons)
                                    {
                                        FormatNonPricedDataLine(purchase.Name, purchase.Description, output,
                                                                sourceJump: purchase.SourceJump, sourceCharacter: purchase.SourceCharacter);

                                        FormatBlankLine(output);
                                    }
                                }
                                break;
                            case "Limitations":
                                if (section.Enabled)
                                {
                                    sectionSubtotal = 0;

                                    currencyAbbreviation = FormatCurrencyAbbreviation("WP");

                                    output.Add(FormatSectionTitle("Limitations"));
                                    FormatBlankLine(output);

                                    foreach (SupplementDrawbackModel drawback in LoadedSave.GenericWarehouse.Limitations)
                                    {
                                        budget += drawback.Value;
                                        sectionSubtotal += drawback.Value;

                                        budgetLastHalf = FormatBudgetLastHalf(drawback.Value, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(drawback.Name, drawback.Description, budgetLastHalf, output);

                                        FormatBlankLine(output);
                                    }
                                }
                                else
                                {
                                    foreach (SupplementDrawbackModel drawback in LoadedSave.GenericWarehouse.Limitations)
                                    {
                                        budget += drawback.Value;
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        FormatSectionEnding(output, section.Enabled, sectionSeparator);
                    }
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    foreach (ExportFormatToggle section in LoadedExportOptions.PersonalRealitySectionList)
                    {
                        switch (section.Name)
                        {
                            case "Point Summary":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Point Summary"));

                                    currencyAbbreviation = FormatCurrencyAbbreviation("WP");

                                    Dictionary<string, int> additionalValues = new()
                                    {
                                        {"Per-Jump WP gained", jumpWP },
                                        {"Investment WP", investmentWP },
                                        {"Universal Drawback WP", drawbackSupplementWP },
                                        {"Limitation WP", supplementLimitationWP },
                                        {"Patient Jumper WP", patientJumperWP }
                                    };

                                    output.Add(FormatPointSummary(LoadedSave.PersonalReality.Budget, additionalValues, currencyAbbreviation));
                                }
                                break;
                            case "Core Mode":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Core Mode"));

                                    line = LoadedSave.PersonalReality.CoreMode.ToString();

                                    if (LoadedSave.PersonalReality.CoreModeDescription != "")
                                    {
                                        line += ": " + LoadedSave.PersonalReality.CoreModeDescription;
                                    }

                                    output.Add(line);
                                }
                                break;
                            case "Purchases":
                                if (section.Enabled)
                                {
                                    List<SupplementPurchase> basics = new();
                                    List<SupplementPurchase> utilities = new();
                                    List<SupplementPurchase> cosmetic = new();
                                    List<SupplementPurchase> facilities = new();
                                    List<SupplementPurchase> extensions = new();
                                    List<SupplementPurchase> equipment = new();
                                    List<SupplementPurchase> companions = new();
                                    List<SupplementPurchase> misc = new();

                                    foreach (SupplementPurchase purchase in LoadedSave.PersonalReality.Purchases)
                                    {
                                        switch (purchase.Category)
                                        {
                                            case "Basics":
                                                basics.Add(purchase);
                                                break;
                                            case "Utilities":
                                                utilities.Add(purchase);
                                                break;
                                            case "Cosmetic":
                                                cosmetic.Add(purchase);
                                                break;
                                            case "Facilities":
                                                facilities.Add(purchase);
                                                break;
                                            case "Extensions":
                                                extensions.Add(purchase);
                                                break;
                                            case "Equipment":
                                                equipment.Add(purchase);
                                                break;
                                            case "Companions":
                                                companions.Add(purchase);
                                                break;
                                            case "Misc":
                                                misc.Add(purchase);
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    output.Add(FormatSectionTitle("Purchases"));
                                    FormatBlankLine(output);

                                    sectionSubtotal = 0;

                                    currencyAbbreviation = FormatCurrencyAbbreviation("WP");

                                    output.Add(FormatSectionTitle("Basics"));

                                    foreach (SupplementPurchase purchase in basics)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Utilities and Structures"));

                                    foreach (SupplementPurchase purchase in utilities)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Cosmetic Upgrades"));

                                    foreach (SupplementPurchase purchase in cosmetic)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Facilities"));

                                    foreach (SupplementPurchase purchase in facilities)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Extensions"));

                                    foreach (SupplementPurchase purchase in extensions)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Items & Equipment"));

                                    foreach (SupplementPurchase purchase in equipment)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Companions"));

                                    foreach (SupplementPurchase purchase in companions)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Misc."));

                                    foreach (SupplementPurchase purchase in misc)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }
                                }
                                else
                                {
                                    foreach (SupplementPurchase purchase in LoadedSave.PersonalReality.Purchases)
                                    {
                                        budget -= purchase.DisplayCost;
                                    }
                                }
                                break;
                            case "Additions":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Warehouse Additions"));

                                    FormatBlankLine(output);

                                    foreach (Purchase purchase in warehouseAddons)
                                    {
                                        FormatNonPricedDataLine(purchase.Name, purchase.Description, output,
                                                                sourceJump: purchase.SourceJump, sourceCharacter: purchase.SourceCharacter);

                                        FormatBlankLine(output);
                                    }
                                }
                                break;
                            case "Limitations":
                                if (section.Enabled)
                                {
                                    sectionSubtotal = 0;

                                    currencyAbbreviation = FormatCurrencyAbbreviation("WP");

                                    output.Add(FormatSectionTitle("Limitations"));
                                    FormatBlankLine(output);

                                    foreach (SupplementDrawbackModel drawback in LoadedSave.PersonalReality.Limitations)
                                    {
                                        budget += drawback.Value;
                                        sectionSubtotal += drawback.Value;

                                        budgetLastHalf = FormatBudgetLastHalf(drawback.Value, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(drawback.Name, drawback.Description, budgetLastHalf, output, drawback.Value);

                                        FormatBlankLine(output);
                                    }
                                }
                                else
                                {
                                    foreach (SupplementDrawbackModel drawback in LoadedSave.PersonalReality.Limitations)
                                    {
                                        budget += drawback.Value;
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        FormatSectionEnding(output, section.Enabled, sectionSeparator);
                    }
                    break;
                default:
                    break;
            }

            FormatClosingLines(output);

            TxtAccess.WriteExportText($"{character.Name} Warehouse Build", "Warehouse", output);
        }

        private void ExportBodyMod(Character character, Options.BodyModSupplements loadedBodyMod)
        {
            int characterIndex = CharacterList.IndexOf(character);

            char sectionSeparator = LoadedExportOptions.SectionSeparator;
            string budgetLastHalf;
            string currencyAbbreviation;

            int budget = 0;
            int sectionSubtotal;

            int drawbackBP = 0;
            int jumpBP = 0;
            int investmentBP = 0;
            int questBP = 0;

            List<Purchase> bodyModAddons = new();

            List<string> output = new();
            string line;

            switch (loadedBodyMod)
            {
                case Options.BodyModSupplements.Generic:
                    jumpBP = CalculateJumpBP(characterIndex);
                    drawbackBP = CalculateDrawbackBP(character.BodyMod);
                    investmentBP = CalculateInvestmentBP(characterIndex);

                    budget = LoadedSave.GenericBodyMod.Budget;
                    budget += drawbackBP;
                    budget += jumpBP;
                    budget += investmentBP;
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    jumpBP = CalculateJumpBP(characterIndex);
                    investmentBP = CalculateInvestmentBP(characterIndex);

                    budget = LoadedSave.SBBodyMod.Budget;
                    budget += jumpBP;
                    budget += investmentBP;
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    jumpBP = CalculateJumpBP(characterIndex);
                    drawbackBP = CalculateDrawbackBP(character.BodyMod);
                    investmentBP = CalculateInvestmentBP(characterIndex);
                    questBP = CalculateQuestBP(characterIndex);

                    budget = LoadedSave.EssentialBodyMod.Budget;
                    budget += drawbackBP;
                    budget += jumpBP;
                    budget += investmentBP;
                    budget += questBP;
                    break;
                default:
                    break;
            }

            foreach (Jump jump in LoadedSave.JumpList)
            {
                if (jump.Build.Count > characterIndex)
                {
                    foreach (Purchase purchase in jump.Build[characterIndex].Purchase)
                    {
                        if (purchase.BodyModAddition)
                        {
                            bodyModAddons.Add(purchase);
                        }
                    }
                }
            }

            FormatOpeningLines(output, $"{character.Name}'s Body Mod");

            switch (loadedBodyMod)
            {
                case Options.BodyModSupplements.Generic:
                    currencyAbbreviation = FormatCurrencyAbbreviation("BP");

                    foreach (ExportFormatToggle section in LoadedExportOptions.BodyModSectionList)
                    {
                        switch (section.Name)
                        {
                            case "Point Summary":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Point Summary"));

                                    Dictionary<string, int> additionalValues = new()
                                    {
                                        {"Per-Jump BP gained", jumpBP },
                                        {"Investment BP", investmentBP },
                                        {"Drawback BP", drawbackBP }
                                    };

                                    output.Add(FormatPointSummary(LoadedSave.GenericBodyMod.Budget, additionalValues, currencyAbbreviation));
                                }
                                break;
                            case "Supplement Details":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Body Mod Details"));

                                    FormatBlankLine(output);

                                    if (!LoadedSave.GenericBodyMod.PurchasesAllowed)
                                    {
                                        output.Add(LoadedSave.GenericBodyMod.Name + ": " + LoadedSave.GenericBodyMod.FullDescription);

                                        FormatBlankLine(output);
                                    }
                                    else
                                    {
                                        output.Add("Name: " + LoadedSave.GenericBodyMod.Name);

                                        FormatBlankLine(output);
                                    }

                                    if (LoadedSave.GenericBodyMod.Version != "")
                                    {
                                        output.Add("Version: " + LoadedSave.GenericBodyMod.Version);

                                        FormatBlankLine(output);
                                    }

                                    if (LoadedSave.GenericBodyMod.Source != "")
                                    {
                                        output.Add("Source: " + LoadedSave.GenericBodyMod.Source);

                                        FormatBlankLine(output);
                                    }

                                    if (LoadedSave.GenericBodyMod.Author != "")
                                    {
                                        output.Add("Author: " + LoadedSave.GenericBodyMod.Author);

                                        FormatBlankLine(output);
                                    }

                                    if (character.BodyMod.SupplementDelay > 0)
                                    {
                                        output.Add("Supplement taken on Jump #" + character.BodyMod.SupplementDelay);

                                        FormatBlankLine(output);
                                    }
                                }
                                break;
                            case "Perks":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Perks"));
                                    FormatBlankLine(output);

                                    sectionSubtotal = 0;

                                    foreach (SupplementPurchase purchase in character.BodyMod.Purchases)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }
                                }
                                else
                                {
                                    foreach (SupplementPurchase purchase in character.BodyMod.Purchases)
                                    {
                                        budget -= purchase.DisplayCost;
                                    }
                                }
                                break;
                            case "Additions":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Body Mod Additions"));

                                    FormatBlankLine(output);

                                    foreach (Purchase purchase in bodyModAddons)
                                    {
                                        FormatNonPricedDataLine(purchase.Name, purchase.Description, output, sourceJump: purchase.SourceJump);

                                        FormatBlankLine(output);
                                    }
                                }
                                break;
                            case "Drawbacks":
                                if (section.Enabled)
                                {
                                    sectionSubtotal = 0;

                                    output.Add(FormatSectionTitle("Drawbacks"));
                                    FormatBlankLine(output);

                                    foreach (SupplementDrawbackModel drawback in character.BodyMod.Limitations)
                                    {
                                        budget += drawback.Value;
                                        sectionSubtotal += drawback.Value;

                                        budgetLastHalf = FormatBudgetLastHalf(drawback.Value, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(drawback.Name, drawback.Description, budgetLastHalf, output, drawback.Value);

                                        FormatBlankLine(output);
                                    }
                                }
                                else
                                {
                                    foreach (SupplementDrawbackModel drawback in character.BodyMod.Limitations)
                                    {
                                        budget += drawback.Value;
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        FormatSectionEnding(output, section.Enabled, sectionSeparator);
                    }
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    currencyAbbreviation = FormatCurrencyAbbreviation("BP");

                    foreach (ExportFormatToggle section in LoadedExportOptions.BodyModSectionList)
                    {
                        switch (section.Name)
                        {
                            case "Point Summary":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Point Summary"));

                                    Dictionary<string, int> additionalValues = new()
                                    {
                                        {"Investment BP", investmentBP },
                                        {"Anomalous Stipend, only for Base Form", character.BodyMod.AnomalousLevel * 100 }
                                    };

                                    output.Add(FormatPointSummary(LoadedSave.SBBodyMod.Budget, additionalValues, currencyAbbreviation));
                                }
                                break;
                            case "Supplement Details":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Body Mod Details"));

                                    FormatBlankLine(output);

                                    output.Add("Name: SB Body Mod");

                                    FormatBlankLine(output);

                                    if (character.BodyMod.SupplementDelay > 0)
                                    {
                                        output.Add("Supplement taken on Jump #" + character.BodyMod.SupplementDelay);

                                        FormatBlankLine(output);
                                    }
                                }
                                break;
                            case "Perks":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Base Form"));
                                    FormatBlankLine(output);

                                    sectionSubtotal = 0;
                                    int anomalousStipend = character.BodyMod.AnomalousLevel * 100;
                                    budget += anomalousStipend;

                                    foreach (SupplementPurchase purchase in character.BodyMod.BaseFormDetails)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;
                                        anomalousStipend -= purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        output.Add(purchase.Category);
                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatSectionEnding(output, section.Enabled, sectionSeparator);

                                    output.Add(FormatSectionTitle("Extra Bits"));
                                    FormatBlankLine(output);

                                    foreach (SupplementPurchase purchase in character.BodyMod.ExtraBitsList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;
                                        anomalousStipend -= purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    if (anomalousStipend > 0)
                                    {
                                        budget -= anomalousStipend;
                                    }

                                    FormatSectionEnding(output, section.Enabled, sectionSeparator);

                                    output.Add(FormatSectionTitle("Affinity & Augments"));
                                    FormatBlankLine(output);

                                    sectionSubtotal = 0;

                                    SupplementPurchase affinity = new();

                                    line = "Affinity: ";

                                    switch (character.BodyMod.AffinityIndex)
                                    {
                                        case 1:
                                            line += "Body";

                                            output.Add(line);

                                            budget -= 100;
                                            sectionSubtotal += 100;

                                            affinity.Name = "Body";
                                            affinity.Description = character.BodyMod.AffinityDescription;
                                            affinity.DisplayCost = 100;

                                            budgetLastHalf = FormatBudgetLastHalf(affinity.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                            FormatPricedDataLine(affinity.Name, affinity.Description, budgetLastHalf, output, affinity.DisplayCost);
                                            break;
                                        case 2:
                                            line += "Mind";

                                            output.Add(line);

                                            budget -= 100;
                                            sectionSubtotal += 100;

                                            affinity.Name = "Mind";
                                            affinity.Description = character.BodyMod.AffinityDescription;
                                            affinity.DisplayCost = 100;

                                            budgetLastHalf = FormatBudgetLastHalf(affinity.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                            FormatPricedDataLine(affinity.Name, affinity.Description, budgetLastHalf, output, affinity.DisplayCost);
                                            break;
                                        case 3:
                                            line += "Heart";

                                            output.Add(line);

                                            budget -= 100;
                                            sectionSubtotal += 100;

                                            affinity.Name = "Heart";
                                            affinity.Description = character.BodyMod.AffinityDescription;
                                            affinity.DisplayCost = 100;

                                            budgetLastHalf = FormatBudgetLastHalf(affinity.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                            FormatPricedDataLine(affinity.Name, affinity.Description, budgetLastHalf, output, affinity.DisplayCost);
                                            break;
                                        default:
                                            line += "None";

                                            output.Add(line);

                                            affinity.Name = "None";
                                            affinity.Description = character.BodyMod.AffinityDescription;
                                            affinity.DisplayCost = 0;

                                            budgetLastHalf = FormatBudgetLastHalf(affinity.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                            FormatPricedDataLine(affinity.Name, affinity.Description, budgetLastHalf, output, affinity.DisplayCost);
                                            break;
                                    }

                                    FormatBlankLine(output);
                                    FormatBlankLine(output);

                                    foreach (AugmentPurchase augment in character.BodyMod.BodyAugmentList)
                                    {
                                        budget -= augment.DisplayCost;
                                        sectionSubtotal += augment.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(augment.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(augment.Name + " " + augment.AugmentLevel, augment.Description, budgetLastHalf, output, augment.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    foreach (AugmentPurchase augment in character.BodyMod.MindAugmentList)
                                    {
                                        budget -= augment.DisplayCost;
                                        sectionSubtotal += augment.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(augment.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(augment.Name + " " + augment.AugmentLevel, augment.Description, budgetLastHalf, output, augment.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    foreach (AugmentPurchase augment in character.BodyMod.HeartAugmentList)
                                    {
                                        budget -= augment.DisplayCost;
                                        sectionSubtotal += augment.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(augment.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(augment.Name + " " + augment.AugmentLevel, augment.Description, budgetLastHalf, output, augment.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatSectionEnding(output, section.Enabled, sectionSeparator);

                                    output.Add(FormatSectionTitle("Powers"));
                                    FormatBlankLine(output);

                                    foreach (SupplementPurchase purchase in character.BodyMod.SBPowerList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }
                                }
                                else
                                {
                                    int anomalousStipend = character.BodyMod.AnomalousLevel * 100;

                                    budget += anomalousStipend;

                                    foreach (SupplementPurchase purchase in character.BodyMod.BaseFormDetails)
                                    {
                                        budget -= purchase.DisplayCost;
                                        anomalousStipend -= purchase.DisplayCost;
                                    }

                                    foreach (SupplementPurchase purchase in character.BodyMod.ExtraBitsList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        anomalousStipend -= purchase.DisplayCost;
                                    }

                                    if (anomalousStipend > 0)
                                    {
                                        budget -= anomalousStipend;
                                    }

                                    if (character.BodyMod.AffinityIndex > 0)
                                    {
                                        budget -= 100;
                                    }

                                    foreach (AugmentPurchase augment in character.BodyMod.BodyAugmentList)
                                    {
                                        budget -= augment.DisplayCost;
                                    }
                                    foreach (AugmentPurchase augment in character.BodyMod.MindAugmentList)
                                    {
                                        budget -= augment.DisplayCost;
                                    }
                                    foreach (AugmentPurchase augment in character.BodyMod.HeartAugmentList)
                                    {
                                        budget -= augment.DisplayCost;
                                    }

                                    foreach (SupplementPurchase purchase in character.BodyMod.SBPowerList)
                                    {
                                        budget -= purchase.DisplayCost;
                                    }
                                }
                                break;
                            case "Additions":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Body Mod Additions"));

                                    FormatBlankLine(output);

                                    foreach (Purchase purchase in bodyModAddons)
                                    {
                                        FormatNonPricedDataLine(purchase.Name, purchase.Description, output, sourceJump: purchase.SourceJump);
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        FormatSectionEnding(output, section.Enabled, sectionSeparator);
                    }
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    currencyAbbreviation = FormatCurrencyAbbreviation("EP");

                    List<SupplementPurchase> EBMBasicPerkList = new();
                    List<SupplementPurchase> EBMPhysicalPerkList = new();
                    List<SupplementPurchase> EBMMentalPerkList = new();
                    List<SupplementPurchase> EBMSpiritualPerkList = new();
                    List<SupplementPurchase> EBMSkillPerkList = new();
                    List<SupplementPurchase> EBMSupernaturalPerkList = new();
                    List<SupplementPurchase> EBMItemPerkList = new();
                    List<SupplementPurchase> EBMCompanionPerkList = new();

                    foreach (SupplementPurchase purchase in character.BodyMod.EBMPurchaseList)
                    {
                        switch (purchase.Category)
                        {
                            case "Basic":
                                EBMBasicPerkList.Add(purchase);
                                break;
                            case "Physical":
                                EBMPhysicalPerkList.Add(purchase);
                                break;
                            case "Mental":
                                EBMMentalPerkList.Add(purchase);
                                break;
                            case "Spiritual":
                                EBMSpiritualPerkList.Add(purchase);
                                break;
                            case "Skill":
                                EBMSkillPerkList.Add(purchase);
                                break;
                            case "Supernatural":
                                EBMSupernaturalPerkList.Add(purchase);
                                break;
                            case "Item":
                                EBMItemPerkList.Add(purchase);
                                break;
                            case "Companion":
                                EBMCompanionPerkList.Add(purchase);
                                break;
                            default:
                                break;
                        }
                    }

                    foreach (ExportFormatToggle section in LoadedExportOptions.BodyModSectionList)
                    {
                        switch (section.Name)
                        {
                            case "Point Summary":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Point Summary"));

                                    Dictionary<string, int> additionalValues = new()
                                    {
                                        {"Per-Jump EP gained", jumpBP },
                                        {"Investment EP", investmentBP },
                                        {"Drawback EP", drawbackBP },
                                        {"Quest EP", questBP }
                                    };

                                    output.Add(FormatPointSummary(LoadedSave.EssentialBodyMod.Budget, additionalValues, currencyAbbreviation));
                                }
                                break;
                            case "Supplement Details":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Body Mod Details"));

                                    FormatBlankLine(output);

                                    output.Add("Name: Essential Body Mod");

                                    FormatBlankLine(output);

                                    string essenceMode = "";
                                    string epAccessMode = "";

                                    switch (LoadedSave.EssentialBodyMod.EssenceMode)
                                    {
                                        case EssentialBodyMod.EssenceModes.NoEssence:
                                            essenceMode = "No Essence";
                                            break;
                                        case EssentialBodyMod.EssenceModes.SingleEssence:
                                            essenceMode = "Single Essence";
                                            break;
                                        case EssentialBodyMod.EssenceModes.DualEssence:
                                            essenceMode = "Dual Essence";
                                            break;
                                        case EssentialBodyMod.EssenceModes.MultiEssence:
                                            essenceMode = "Multi Essence";
                                            break;
                                        default:
                                            break;
                                    }
                                    switch (LoadedSave.EssentialBodyMod.EPAccessMode)
                                    {
                                        case EssentialBodyMod.EPAccessModes.NoAccess:
                                            epAccessMode = "No Access";
                                            break;
                                        case EssentialBodyMod.EPAccessModes.LesserAccess:
                                            epAccessMode = "Lesser Access";
                                            break;
                                        case EssentialBodyMod.EPAccessModes.StandardAccess:
                                            epAccessMode = "Standard Access";
                                            break;
                                        default:
                                            break;
                                    }

                                    output.Add("Starting Mode: " + LoadedSave.EssentialBodyMod.StartingMode.ToString());
                                    output.Add("Essence Mode: " + essenceMode);
                                    output.Add("Advancement Mode: " + LoadedSave.EssentialBodyMod.AdvancementMode.ToString());

                                    line = "EP Access Mode: " + epAccessMode;
                                    if (LoadedSave.EssentialBodyMod.CumulativeAccess)
                                    {
                                        line += " (Cumulative)";
                                    }

                                    output.Add(line);

                                    FormatBlankLine(output);

                                    if (character.BodyMod.EBMEssenceList.Any())
                                    {
                                        switch (LoadedSave.EssentialBodyMod.EssenceMode)
                                        {
                                            case EssentialBodyMod.EssenceModes.SingleEssence:
                                                output.Add("Essence selection: " + character.BodyMod.EBMEssenceList[0].Name);
                                                output.Add("Essence description: " + character.BodyMod.EBMEssenceList[0].Description);
                                                break;
                                            case EssentialBodyMod.EssenceModes.DualEssence:

                                                output.Add("Essence selection: " + character.BodyMod.EBMEssenceList[0].Name);
                                                output.Add("Essence description: " + character.BodyMod.EBMEssenceList[0].Description);

                                                if (character.BodyMod.EBMEssenceList.Count > 1)
                                                {
                                                    FormatBlankLine(output);
                                                    output.Add("Essence selection: " + character.BodyMod.EBMEssenceList[1].Name);
                                                    output.Add("Essence description: " + character.BodyMod.EBMEssenceList[1].Description);
                                                }
                                                break;
                                            case EssentialBodyMod.EssenceModes.MultiEssence:
                                                foreach (EBMEssence essence in character.BodyMod.EBMEssenceList)
                                                {
                                                    output.Add("Essence selection: " + essence.Name);
                                                    output.Add("Essence description: " + essence.Description);

                                                    FormatBlankLine(output);
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    FormatBlankLine(output);

                                    if (LoadedSave.EssentialBodyMod.AdvancementMode == EssentialBodyMod.AdvancementModes.Questing)
                                    {
                                        output.Add("Quests completed:");
                                        output.Add("Minor: " + character.BodyMod.EBMMinorQuests);
                                        output.Add("Major: " + character.BodyMod.EBMMajorQuests);
                                    }

                                    if (character.BodyMod.SupplementDelay > 0)
                                    {
                                        output.Add("Supplement taken on Jump #" + character.BodyMod.SupplementDelay);

                                        FormatBlankLine(output);
                                    }
                                }
                                break;
                            case "Perks":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Perks"));
                                    FormatBlankLine(output);

                                    sectionSubtotal = 0;

                                    output.Add(FormatSectionTitle("Basic Perks"));

                                    foreach (SupplementPurchase purchase in EBMBasicPerkList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Physical Perks"));

                                    foreach (SupplementPurchase purchase in EBMPhysicalPerkList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Mental Perks"));

                                    foreach (SupplementPurchase purchase in EBMMentalPerkList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Spiritual Perks"));

                                    foreach (SupplementPurchase purchase in EBMSpiritualPerkList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Skill Perks"));

                                    foreach (SupplementPurchase purchase in EBMSkillPerkList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Supernatural Perks"));

                                    foreach (SupplementPurchase purchase in EBMSupernaturalPerkList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Item Perks"));

                                    foreach (SupplementPurchase purchase in EBMItemPerkList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }

                                    FormatBlankLine(output);
                                    output.Add(FormatSectionTitle("Companion Perks"));

                                    foreach (SupplementPurchase purchase in EBMCompanionPerkList)
                                    {
                                        budget -= purchase.DisplayCost;
                                        sectionSubtotal += purchase.DisplayCost;

                                        budgetLastHalf = FormatBudgetLastHalf(purchase.DisplayCost, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(purchase.Name, purchase.Description, budgetLastHalf, output, purchase.DisplayCost);

                                        FormatBlankLine(output);
                                    }
                                }
                                else
                                {
                                    foreach (SupplementPurchase purchase in character.BodyMod.EBMPurchaseList)
                                    {
                                        budget -= purchase.DisplayCost;
                                    }
                                }
                                break;
                            case "Additions":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Body Mod Additions"));

                                    FormatBlankLine(output);

                                    foreach (Purchase purchase in bodyModAddons)
                                    {
                                        FormatNonPricedDataLine(purchase.Name, purchase.Description, output, sourceJump: purchase.SourceJump);
                                    }
                                }
                                break;
                            case "Drawbacks":
                                if (section.Enabled)
                                {
                                    sectionSubtotal = 0;

                                    output.Add(FormatSectionTitle("Drawbacks"));
                                    FormatBlankLine(output);

                                    foreach (SupplementDrawbackModel drawback in character.BodyMod.EBMDrawbackList)
                                    {
                                        budget += drawback.Value;
                                        sectionSubtotal += drawback.Value;

                                        budgetLastHalf = FormatBudgetLastHalf(drawback.Value, budget, sectionSubtotal, currencyAbbreviation);

                                        FormatPricedDataLine(drawback.Name, drawback.Description, budgetLastHalf, output, drawback.Value);

                                        FormatBlankLine(output);
                                    }
                                }
                                else
                                {
                                    foreach (SupplementDrawbackModel drawback in character.BodyMod.EBMDrawbackList)
                                    {
                                        budget += drawback.Value;
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        FormatSectionEnding(output, section.Enabled, sectionSeparator);
                    }
                    break;
                default:
                    break;
            }

            FormatClosingLines(output);

            TxtAccess.WriteExportText($"{character.Name} Body Mod", "Body Mods", output);
        }

        private void ExportDrawbackSupplement(Options.DrawbackSupplements loadedDrawbackSupplement)
        {
            Dictionary<string, string> budgetLastHalves = new()
            {
                {"Choice Points", ""},
                {"Warehouse Points", ""},
                {"Companion Choice Points", ""},
                {"Item Choice Points", ""}
            };
            string budgetLastHalf;

            Dictionary<string, int> drawbackValues = new()
            {
                {"Choice Points", 0},
                {"Warehouse Points", 0},
                {"Companion Choice Points", 0},
                {"Item Choice Points", 0}
            };
            Dictionary<string, int> drawbackTotals = new()
            {
                {"Choice Points", 0},
                {"Warehouse Points", 0},
                {"Companion Choice Points", 0},
                {"Item Choice Points", 0}
            };

            List<DrawbackSupplementPurchase> drawbacks = new();
            List<HouseRuleModel> houseRules = new();

            List<string> output = new();
            string line;

            switch (loadedDrawbackSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    foreach (DrawbackSupplementPurchase drawback in LoadedSave.GenericDrawbackSupplement.Purchases)
                    {
                        drawbacks.Add(drawback);
                    }

                    foreach (HouseRuleModel houseRule in LoadedSave.GenericDrawbackSupplement.HouseRules)
                    {
                        houseRules.Add(houseRule);
                    }

                    if (LoadedSave.GenericDrawbackSupplement.Name != "")
                    {
                        FormatOpeningLines(output, $"{LoadedSave.GenericDrawbackSupplement.Name}");
                    }
                    else
                    {
                        FormatOpeningLines(output, "Drawback Supplement");
                    }

                    foreach (ExportFormatToggle section in LoadedExportOptions.DrawbackSupplementSectionList)
                    {
                        switch (section.Name)
                        {
                            case "Point Summary":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Point summary"));

                                    FormatBlankLine(output);

                                    output.Add("CP Gained: " + LoadedSave.GenericDrawbackSupplement.CPGained);
                                    output.Add("Companion CP Gained: " + LoadedSave.GenericDrawbackSupplement.CPCompanionGained);
                                    output.Add("Item CP Gained: " + LoadedSave.GenericDrawbackSupplement.CPItemGained);
                                    output.Add("WP Gained (One time): " + LoadedSave.GenericDrawbackSupplement.WPGained);
                                }
                                break;
                            case "Supplement Details":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Supplement details"));

                                    FormatBlankLine(output);

                                    FormatDocumentDetails(LoadedSave.GenericDrawbackSupplement, output);

                                    if (LoadedSave.GenericDrawbackSupplement.AllowedDuringGauntlets && LoadedSave.GenericDrawbackSupplement.HalvedPointsDuringGauntlets)
                                    {
                                        FormatBlankLine(output);
                                        output.Add("Half points gained during Gauntlets.");
                                    }
                                    else if (LoadedSave.GenericDrawbackSupplement.AllowedDuringGauntlets)
                                    {
                                        output.Add("Full points gained during Gauntlets.");
                                    }
                                }
                                break;
                            case "House Rules":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("House Rules"));

                                    FormatBlankLine(output);

                                    foreach (HouseRuleModel houseRule in houseRules)
                                    {
                                        FormatNonPricedDataLine(houseRule.HouseRuleName, houseRule.HouseRuleDescription, output);
                                    }
                                }
                                break;
                            case "Drawbacks":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Universal Drawbacks"));

                                    FormatBlankLine(output);

                                    foreach (DrawbackSupplementPurchase drawback in LoadedSave.GenericDrawbackSupplement.Purchases)
                                    {
                                        drawbackTotals["Choice Points"] += drawback.ValueChoicePoints;
                                        drawbackTotals["Warehouse Points"] += drawback.ValueWarehousePoints;
                                        drawbackTotals["Companion Choice Points"] += drawback.ValueCompanionPoints;
                                        drawbackTotals["Item Choice Points"] += drawback.ValueItemPoints;

                                        drawbackValues["Choice Points"] = drawback.ValueChoicePoints;
                                        drawbackValues["Warehouse Points"] = drawback.ValueWarehousePoints;
                                        drawbackValues["Companion Choice Points"] = drawback.ValueCompanionPoints;
                                        drawbackValues["Item Choice Points"] = drawback.ValueItemPoints;

                                        budgetLastHalf = FormatUDSBudgetLastHalf(budgetLastHalves, drawbackValues, drawbackTotals);

                                        FormatPricedDataLine(drawback.Name, drawback.Description, budgetLastHalf, output);

                                        FormatBlankLine(output);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case Options.DrawbackSupplements.UDS:
                    foreach (DrawbackSupplementPurchase drawback in LoadedSave.UniversalDrawbackSupplement.Purchases)
                    {
                        drawbacks.Add(drawback);
                    }

                    foreach (HouseRuleModel houseRule in LoadedSave.UniversalDrawbackSupplement.HouseRules)
                    {
                        houseRules.Add(houseRule);
                    }

                    FormatOpeningLines(output, "Universal Drawback Supplement");

                    foreach (ExportFormatToggle section in LoadedExportOptions.DrawbackSupplementSectionList)
                    {
                        switch (section.Name)
                        {
                            case "Point Summary":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Point summary"));

                                    FormatBlankLine(output);

                                    output.Add("CP Gained: " + LoadedSave.UniversalDrawbackSupplement.CPGained);
                                    output.Add("Companion CP Gained: " + LoadedSave.UniversalDrawbackSupplement.CPCompanionGained);
                                    output.Add("Item CP Gained: " + LoadedSave.UniversalDrawbackSupplement.CPItemGained);
                                    output.Add("WP Gained (One time): " + LoadedSave.UniversalDrawbackSupplement.WPGained);
                                }
                                break;
                            case "Supplement Details":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Supplement details"));

                                    FormatBlankLine(output);

                                    FormatDocumentDetails(LoadedSave.UniversalDrawbackSupplement, output);

                                    if (LoadedSave.UniversalDrawbackSupplement.AllowedDuringGauntlets && LoadedSave.UniversalDrawbackSupplement.HalvedPointsDuringGauntlets)
                                    {
                                        FormatBlankLine(output);
                                        output.Add("Half points gained during Gauntlets.");
                                    }
                                    else if (LoadedSave.UniversalDrawbackSupplement.AllowedDuringGauntlets)
                                    {
                                        output.Add("Full points gained during Gauntlets.");
                                    }
                                }
                                break;
                            case "House Rules":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("House Rules"));

                                    FormatBlankLine(output);

                                    foreach (HouseRuleModel houseRule in houseRules)
                                    {
                                        FormatNonPricedDataLine(houseRule.HouseRuleName, houseRule.HouseRuleDescription, output);
                                    }
                                }
                                break;
                            case "Drawbacks":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Universal Drawbacks"));

                                    FormatBlankLine(output);

                                    foreach (DrawbackSupplementPurchase drawback in LoadedSave.UniversalDrawbackSupplement.Purchases)
                                    {
                                        drawbackTotals["Choice Points"] += drawback.ValueChoicePoints;
                                        drawbackTotals["Warehouse Points"] += drawback.ValueWarehousePoints;
                                        drawbackTotals["Companion Choice Points"] += drawback.ValueCompanionPoints;
                                        drawbackTotals["Item Choice Points"] += drawback.ValueItemPoints;

                                        drawbackValues["Choice Points"] = drawback.ValueChoicePoints;
                                        drawbackValues["Warehouse Points"] = drawback.ValueWarehousePoints;
                                        drawbackValues["Companion Choice Points"] = drawback.ValueCompanionPoints;
                                        drawbackValues["Item Choice Points"] = drawback.ValueItemPoints;

                                        budgetLastHalf = FormatUDSBudgetLastHalf(budgetLastHalves, drawbackValues, drawbackTotals);

                                        FormatPricedDataLine(drawback.Name, drawback.Description, budgetLastHalf, output);

                                        FormatBlankLine(output);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case Options.DrawbackSupplements.UU:
                    foreach (DrawbackSupplementPurchase drawback in LoadedSave.UUSupplement.Purchases)
                    {
                        drawbacks.Add(drawback);
                    }

                    foreach (HouseRuleModel houseRule in LoadedSave.UUSupplement.HouseRules)
                    {
                        houseRules.Add(houseRule);
                    }

                    FormatOpeningLines(output, "U.U. Supplement");

                    foreach (ExportFormatToggle section in LoadedExportOptions.DrawbackSupplementSectionList)
                    {
                        switch (section.Name)
                        {
                            case "Point Summary":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Point summary"));

                                    FormatBlankLine(output);

                                    output.Add("CP Gained: " + LoadedSave.UUSupplement.CPGained);
                                    output.Add("Companion CP Gained: " + LoadedSave.UUSupplement.CPCompanionGained);
                                    output.Add("Item CP Gained: " + LoadedSave.UUSupplement.CPItemGained);
                                    output.Add("WP Gained (One time): " + LoadedSave.UUSupplement.WPGained);
                                }
                                break;
                            case "Supplement Details":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Supplement details"));

                                    FormatBlankLine(output);

                                    FormatDocumentDetails(LoadedSave.UUSupplement, output);

                                    if (LoadedSave.UUSupplement.AllowedDuringGauntlets && LoadedSave.UUSupplement.HalvedPointsDuringGauntlets)
                                    {
                                        FormatBlankLine(output);
                                        output.Add("Half points gained during Gauntlets.");
                                        FormatBlankLine(output);
                                    }
                                    else if (LoadedSave.UUSupplement.AllowedDuringGauntlets)
                                    {
                                        output.Add("Full points gained during Gauntlets.");
                                        FormatBlankLine(output);
                                    }

                                    if (LoadedSave.UUSupplement.Mode != "")
                                    {
                                        line = "Mode: " + LoadedSave.UUSupplement.Mode;

                                        if (LoadedSave.UUSupplement.ModeDescription != "")
                                        {
                                            line += " - " + LoadedSave.UUSupplement.ModeDescription;
                                        }

                                        output.Add(line);
                                        FormatBlankLine(output);
                                    }

                                    if (LoadedSave.UUSupplement.RiskLevel != "")
                                    {
                                        line = "Risk Level: " + LoadedSave.UUSupplement.RiskLevel;

                                        if (LoadedSave.UUSupplement.RiskLevelDescription != "")
                                        {
                                            line += " - " + LoadedSave.UUSupplement.RiskLevelDescription;
                                        }

                                        output.Add(line);
                                    }

                                }
                                break;
                            case "House Rules":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("House Rules"));

                                    FormatBlankLine(output);

                                    foreach (HouseRuleModel houseRule in houseRules)
                                    {
                                        FormatNonPricedDataLine(houseRule.HouseRuleName, houseRule.HouseRuleDescription, output);
                                    }
                                }
                                break;
                            case "Drawbacks":
                                if (section.Enabled)
                                {
                                    output.Add(FormatSectionTitle("Universal Drawbacks"));

                                    FormatBlankLine(output);

                                    foreach (DrawbackSupplementPurchase drawback in LoadedSave.UUSupplement.Purchases)
                                    {
                                        drawbackTotals["Choice Points"] += drawback.ValueChoicePoints;
                                        drawbackTotals["Warehouse Points"] += drawback.ValueWarehousePoints;
                                        drawbackTotals["Companion Choice Points"] += drawback.ValueCompanionPoints;
                                        drawbackTotals["Item Choice Points"] += drawback.ValueItemPoints;

                                        drawbackValues["Choice Points"] = drawback.ValueChoicePoints;
                                        drawbackValues["Warehouse Points"] = drawback.ValueWarehousePoints;
                                        drawbackValues["Companion Choice Points"] = drawback.ValueCompanionPoints;
                                        drawbackValues["Item Choice Points"] = drawback.ValueItemPoints;

                                        budgetLastHalf = FormatUDSBudgetLastHalf(budgetLastHalves, drawbackValues, drawbackTotals);

                                        FormatPricedDataLine(drawback.Name, drawback.Description, budgetLastHalf, output);

                                        FormatBlankLine(output);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }

            FormatClosingLines(output);

            TxtAccess.WriteExportText($"{CharacterList[0].Name} Universal Drawbacks", "Universal Drawbacks", output);
        }
        #endregion

        #region Commands
        [RelayCommand]
        private void ExportAllJumpData()
        {
            ExportJumpData(JumpSelection);

            foreach (Character character in CharacterList)
            {
                ExportBuild(JumpSelection, CharacterList.IndexOf(character));
            }
        }

        [RelayCommand]
        private void ExportSelectedBuild()
        {
            if (CharacterSelectionIndex != -1 && JumpSelection != null)
            {
                ListValidationClass.CheckBuildCount(JumpSelection, CharacterSelectionIndex);

                if (CharacterSelectionIndex == 0 && LoadedExportOptions.CompanionBuilds)
                {
                    for (int i = 0; i < CharacterList.Count; i++)
                    {
                        ExportBuild(JumpSelection, i);
                    }
                }
                else
                {
                    ExportBuild(JumpSelection, CharacterSelectionIndex);
                }
            }
        }

        [RelayCommand]
        private void ExportAllProfileData()
        {
            if (CharacterSelection != null)
            {
                ExportProfile(CharacterSelection, CharacterSelectionIndex);
            }
        }

        [RelayCommand]
        private void ExportAllCharacterProfiles()
        {
            foreach (Character character in CharacterList)
            {
                ExportProfile(character, CharacterList.IndexOf(character));
            }
        }

        [RelayCommand]
        private void ExportAllWarehouseData() => ExportWarehouse(CharacterList[0], LoadedOptions.CosmicWarehouseSetting);

        [RelayCommand]
        private void ExportBodyModData()
        {
            if (CharacterSelection != null)
            {
                ExportBodyMod(CharacterSelection, LoadedOptions.BodyModSetting);
            }
        }

        [RelayCommand]
        private void ExportAllBodyMods()
        {
            foreach (Character character in CharacterList)
            {
                ExportBodyMod(character, LoadedOptions.BodyModSetting);
            }
        }

        [RelayCommand]
        private void ExportDrawbackSupplementData() => ExportDrawbackSupplement(LoadedOptions.DrawbackSupplementSetting);

        [RelayCommand]
        private void ExportAllData()
        {
            foreach (Character character in CharacterList)
            {
                ExportProfile(character, CharacterList.IndexOf(character));
                ExportBodyMod(character, LoadedOptions.BodyModSetting);
            }

            foreach (Jump jump in JumpList)
            {
                ExportJumpData(jump);

                foreach (Character character in CharacterList)
                {
                    if (jump.Build.Count >= CharacterList.IndexOf(character) + 1 && jump.Build[CharacterList.IndexOf(character)] != null)
                    {
                        ExportBuild(jump, CharacterList.IndexOf(character));
                    }
                }
            }

            ExportWarehouse(CharacterList[0], LoadedOptions.CosmicWarehouseSetting);
            ExportDrawbackSupplement(LoadedOptions.DrawbackSupplementSetting);
        }

        [RelayCommand(CanExecute = nameof(CanMoveBuildSectionUp))]
        private void MoveBuildSectionUp()
        {
            int index = BuildSectionSelectionIndex;
            ListOperationsClass.SwapListItems(LoadedExportOptions.BuildSectionList, index, index - 1);
            ListOperationsClass.SwapCollectionItems(BuildSectionList, index, index - 1);

            BuildSectionSelectionIndex = index - 1;

            MoveBuildSectionUpCommand.NotifyCanExecuteChanged();
            MoveBuildSectionDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveBuildSectionUp() => BuildSectionSelectionIndex != -1 && BuildSectionSelectionIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveBuildSectionDown))]
        private void MoveBuildSectionDown()
        {
            int index = BuildSectionSelectionIndex;
            ListOperationsClass.SwapListItems(LoadedExportOptions.BuildSectionList, index, index + 1);
            ListOperationsClass.SwapCollectionItems(BuildSectionList, index, index + 1);

            BuildSectionSelectionIndex = index + 1;

            MoveBuildSectionUpCommand.NotifyCanExecuteChanged();
            MoveBuildSectionDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveBuildSectionDown() => BuildSectionSelectionIndex != -1 && BuildSectionSelectionIndex < LoadedExportOptions.BuildSectionList.Count - 1;

        [RelayCommand(CanExecute = nameof(CanMoveProfileSectionUp))]
        private void MoveProfileSectionUp()
        {
            int index = ProfileSectionSelectionIndex;
            ListOperationsClass.SwapListItems(LoadedExportOptions.ProfileSectionList, index, index - 1);
            ListOperationsClass.SwapCollectionItems(ProfileSectionList, index, index - 1);

            ProfileSectionSelectionIndex = index - 1;

            MoveProfileSectionUpCommand.NotifyCanExecuteChanged();
            MoveProfileSectionDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveProfileSectionUp() => ProfileSectionSelectionIndex != -1 && ProfileSectionSelectionIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveProfileSectionDown))]
        private void MoveProfileSectionDown()
        {
            int index = ProfileSectionSelectionIndex;
            ListOperationsClass.SwapListItems(LoadedExportOptions.ProfileSectionList, index, index + 1);
            ListOperationsClass.SwapCollectionItems(ProfileSectionList, index, index + 1);

            ProfileSectionSelectionIndex = index + 1;

            MoveProfileSectionUpCommand.NotifyCanExecuteChanged();
            MoveProfileSectionDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveProfileSectionDown() => ProfileSectionSelectionIndex != -1 && ProfileSectionSelectionIndex < LoadedExportOptions.ProfileSectionList.Count - 1;

        [RelayCommand(CanExecute = nameof(CanMoveProfileSubsectionUp))]
        private void MoveProfileSubsectionUp()
        {
            int index = ProfileSubsectionSelectionIndex;
            ListOperationsClass.SwapListItems(LoadedExportOptions.ProfileSubsectionList, index, index - 1);
            ListOperationsClass.SwapCollectionItems(ProfileSubsectionList, index, index - 1);

            ProfileSubsectionSelectionIndex = index - 1;

            MoveProfileSubsectionUpCommand.NotifyCanExecuteChanged();
            MoveProfileSubsectionDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveProfileSubsectionUp() => ProfileSubsectionSelectionIndex != -1 && ProfileSubsectionSelectionIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveProfileSubsectionDown))]
        private void MoveProfileSubsectionDown()
        {
            int index = ProfileSubsectionSelectionIndex;
            ListOperationsClass.SwapListItems(LoadedExportOptions.ProfileSubsectionList, index, index + 1);
            ListOperationsClass.SwapCollectionItems(ProfileSubsectionList, index, index + 1);

            ProfileSubsectionSelectionIndex = index + 1;

            MoveProfileSubsectionUpCommand.NotifyCanExecuteChanged();
            MoveProfileSubsectionDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveProfileSubsectionDown() => ProfileSubsectionSelectionIndex != -1 && ProfileSubsectionSelectionIndex < LoadedExportOptions.ProfileSubsectionList.Count - 1;

        [RelayCommand(CanExecute = nameof(CanMoveWarehouseSectionUp))]
        private void MoveWarehouseSectionUp()
        {
            if (GenericWarehouseSelected)
            {
                int index = GenericWarehouseSectionSelectionIndex;
                ListOperationsClass.SwapListItems(LoadedExportOptions.GenericWarehouseSectionList, index, index - 1);
                ListOperationsClass.SwapCollectionItems(GenericWarehouseSectionList, index, index - 1);

                GenericWarehouseSectionSelectionIndex = index - 1;

                MoveWarehouseSectionUpCommand.NotifyCanExecuteChanged();
                MoveWarehouseSectionDownCommand.NotifyCanExecuteChanged();
            }
            else if (PersonalRealitySelected)
            {
                int index = PersonalRealitySectionSelectionIndex;
                ListOperationsClass.SwapListItems(LoadedExportOptions.PersonalRealitySectionList, index, index - 1);
                ListOperationsClass.SwapCollectionItems(PersonalRealitySectionList, index, index - 1);

                PersonalRealitySectionSelectionIndex = index - 1;

                MoveWarehouseSectionUpCommand.NotifyCanExecuteChanged();
                MoveWarehouseSectionDownCommand.NotifyCanExecuteChanged();
            }
        }


        private bool CanMoveWarehouseSectionUp()
        {
            if (GenericWarehouseSelected)
            {
                return GenericWarehouseSectionSelectionIndex != -1 && GenericWarehouseSectionSelectionIndex > 0;
            }
            else if (PersonalRealitySelected)
            {
                return PersonalRealitySectionSelectionIndex != -1 && PersonalRealitySectionSelectionIndex > 0;
            }
            else
            {
                return false;
            }
        }

        [RelayCommand(CanExecute = nameof(CanMoveWarehouseSectionDown))]
        private void MoveWarehouseSectionDown()
        {
            if (GenericWarehouseSelected)
            {
                int index = GenericWarehouseSectionSelectionIndex;
                ListOperationsClass.SwapListItems(LoadedExportOptions.GenericWarehouseSectionList, index, index + 1);
                ListOperationsClass.SwapCollectionItems(GenericWarehouseSectionList, index, index + 1);

                GenericWarehouseSectionSelectionIndex = index + 1;

                MoveWarehouseSectionUpCommand.NotifyCanExecuteChanged();
                MoveWarehouseSectionDownCommand.NotifyCanExecuteChanged();
            }
            else if (PersonalRealitySelected)
            {
                int index = PersonalRealitySectionSelectionIndex;
                ListOperationsClass.SwapListItems(LoadedExportOptions.PersonalRealitySectionList, index, index + 1);
                ListOperationsClass.SwapCollectionItems(PersonalRealitySectionList, index, index + 1);

                PersonalRealitySectionSelectionIndex = index + 1;

                MoveWarehouseSectionUpCommand.NotifyCanExecuteChanged();
                MoveWarehouseSectionDownCommand.NotifyCanExecuteChanged();
            }

        }


        private bool CanMoveWarehouseSectionDown()
        {
            if (GenericWarehouseSelected)
            {
                return GenericWarehouseSectionSelectionIndex != -1 && GenericWarehouseSectionSelectionIndex < LoadedExportOptions.GenericWarehouseSectionList.Count - 1;
            }
            else if (PersonalRealitySelected)
            {
                return PersonalRealitySectionSelectionIndex != -1 && PersonalRealitySectionSelectionIndex < LoadedExportOptions.PersonalRealitySectionList.Count - 1;
            }
            else
            {
                return false;
            }
        }

        [RelayCommand(CanExecute = nameof(CanMoveBodyModSectionUp))]
        private void MoveBodyModSectionUp()
        {
            int index = BodyModSectionSelectionIndex;
            ListOperationsClass.SwapListItems(LoadedExportOptions.BodyModSectionList, index, index - 1);
            ListOperationsClass.SwapCollectionItems(BodyModSectionList, index, index - 1);

            BodyModSectionSelectionIndex = index - 1;

            MoveBodyModSectionUpCommand.NotifyCanExecuteChanged();
            MoveBodyModSectionDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveBodyModSectionUp() => BodyModSectionSelectionIndex != -1 && BodyModSectionSelectionIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveBodyModSectionDown))]
        private void MoveBodyModSectionDown()
        {
            int index = BodyModSectionSelectionIndex;
            ListOperationsClass.SwapListItems(LoadedExportOptions.BodyModSectionList, index, index + 1);
            ListOperationsClass.SwapCollectionItems(BodyModSectionList, index, index + 1);

            BodyModSectionSelectionIndex = index + 1;

            MoveBodyModSectionUpCommand.NotifyCanExecuteChanged();
            MoveBodyModSectionDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveBodyModSectionDown() => BodyModSectionSelectionIndex != -1 && BodyModSectionSelectionIndex < LoadedExportOptions.BodyModSectionList.Count - 1;

        [RelayCommand(CanExecute = nameof(CanMoveDrawbackSupplementSectionUp))]
        private void MoveDrawbackSupplementSectionUp()
        {
            int index = DrawbackSupplementSectionSelectionIndex;
            ListOperationsClass.SwapListItems(LoadedExportOptions.DrawbackSupplementSectionList, index, index - 1);
            ListOperationsClass.SwapCollectionItems(DrawbackSupplementSectionList, index, index - 1);

            DrawbackSupplementSectionSelectionIndex = index - 1;

            MoveDrawbackSupplementSectionUpCommand.NotifyCanExecuteChanged();
            MoveDrawbackSupplementSectionDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveDrawbackSupplementSectionUp() => DrawbackSupplementSectionSelectionIndex != -1 && DrawbackSupplementSectionSelectionIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveDrawbackSupplementSectionDown))]
        private void MoveDrawbackSupplementSectionDown()
        {
            int index = DrawbackSupplementSectionSelectionIndex;
            ListOperationsClass.SwapListItems(LoadedExportOptions.DrawbackSupplementSectionList, index, index + 1);
            ListOperationsClass.SwapCollectionItems(DrawbackSupplementSectionList, index, index + 1);

            DrawbackSupplementSectionSelectionIndex = index + 1;

            MoveDrawbackSupplementSectionUpCommand.NotifyCanExecuteChanged();
            MoveDrawbackSupplementSectionDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveDrawbackSupplementSectionDown() => DrawbackSupplementSectionSelectionIndex != -1 && DrawbackSupplementSectionSelectionIndex < LoadedExportOptions.DrawbackSupplementSectionList.Count - 1;
        #endregion
    }
}
