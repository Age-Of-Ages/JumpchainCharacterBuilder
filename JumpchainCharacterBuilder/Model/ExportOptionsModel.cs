using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    public partial class ExportOptions : ObservableValidator
    {
        /// <summary>
        /// Represents the index of the format that the budgeting for exported Builds should be displayed in.
        /// </summary>
        [ObservableProperty]
        private int _budgetFormat = 0;

        /// <summary>
        /// Represents if the name and cost/budget sections should be reversed for exported Builds.
        /// </summary>
        [ObservableProperty]
        private bool _reverseBudgetFormat = false;

        /// <summary>
        /// Represents the single character that will be used to generate section separators.
        /// </summary>
        [ObservableProperty]
        private char _sectionSeparator = '=';

        /// <summary>
        /// Represents the two characters that should be used to enclose budget details and other similar "boxes".
        /// </summary>
        [ObservableProperty]
        private string _budgetEnclosingFormat = "[]";

        /// <summary>
        /// Represents the character that should be used to separate the sides of budget details and other similar "boxes".
        /// </summary>
        [ObservableProperty]
        private char _budgetSeparatorFormat = '/';

        /// <summary>
        /// Represents the list of export sections, for the purposes of ordering Jump Builds.
        /// The order of items in this list determines the order in which build sections will be printed.
        /// 'Perks' include all non-Item purchase types.
        /// 'Items' include all Item purchase types.
        /// Bool value is for if the section should be included in the export or not.
        /// </summary>
        [ObservableProperty]
        private List<ExportFormatToggle> _buildSectionList = new()
        {
            new("Bank Details", true),
            new("Point Summary", true),
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

        /// <summary>
        /// Represents the list of export sections, for the purposes of ordering Character Profile Exports.
        /// The order of items in this list determines the order in which Profile sections will be printed.
        /// 'Profile' will print all sections active in the Profile subsection list.
        /// Bool value is for if the section should be included in the export or not.
        /// </summary>
        [ObservableProperty]
        private List<ExportFormatToggle> _profileSectionList = new()
        {
            new("Profile", true),
            new("Alt Forms", true),
            new("Perks", true),
            new("Items", true),
            new("Attributes", true),
            new("Skills", true),
            new("Learning Rates", true)
        };

        /// <summary>
        /// Represents the list of export sections, for the purposes of ordering Profile subsections in the export.
        /// The order of items in this list determines the order in which Profile subsections will be printed.
        /// Bool value is for if the section should be included in the export or not.
        /// </summary>
        [ObservableProperty]
        private List<ExportFormatToggle> _profileSubsectionList = new()
        {
            new("Biography", true),
            new("Physical Characteristics", true),
            new("Personality", true),
            new("Physical Description", true),
            new("Traits", true)
        };

        /// <summary>
        /// Represents the list of export sections, for the purposes of ordering Warehouse Exports.
        /// The order of items in this list determines the order in which Warehouse sections will be printed.
        /// Bool value is for if the section should be included in the export or not.
        /// </summary>
        [ObservableProperty]
        private List<ExportFormatToggle> _genericWarehouseSectionList = new()
        {
            new("Point Summary", true),
            new("Block Description", true),
            new("Purchases", true),
            new("Additions", true),
            new("Limitations", true)
        };

        /// <summary>
        /// Represents the list of export sections, for the purposes of ordering Warehouse Exports.
        /// The order of items in this list determines the order in which Warehouse sections will be printed.
        /// Bool value is for if the section should be included in the export or not.
        /// </summary>
        [ObservableProperty]
        private List<ExportFormatToggle> _personalRealitySectionList = new()
        {
            new("Point Summary", true),
            new("Core Mode", true),
            new("Purchases", true),
            new("Additions", true),
            new("Limitations", true)
        };

        /// <summary>
        /// Represents the list of export sections, for the purposes of ordering Body Mod Exports.
        /// The order of items in this list determines the order in which Body Mod sections will be printed.
        /// Bool value is for if the section should be included in the export or not.
        /// </summary>
        [ObservableProperty]
        private List<ExportFormatToggle> _bodyModSectionList = new()
        {
            new("Point Summary", true),
            new("Supplement Details", true),
            new("Perks", true),
            new("Additions", true),
            new("Drawbacks", true)
        };

        /// <summary>
        /// Represents the list of export sections, for the purposes of ordering Drawback Supplement Exports.
        /// The order of items in this list determines the order in which Drawback Supplement sections will be printed.
        /// Bool value is for if the section should be included in the export or not.
        /// </summary>
        [ObservableProperty]
        private List<ExportFormatToggle> _drawbackSupplementSectionList = new()
        {
            new("Point Summary", true),
            new("Supplement Details", true),
            new("House Rules", true),
            new("Drawbacks", true)
        };

        /// <summary>
        /// Represents whether Companion's Builds should be exported alongside the Jumper's Build or not.
        /// </summary>
        [ObservableProperty]
        private bool _companionBuilds = true;

        /// <summary>
        /// Represents which export mode to use. This is primarily used for the formatting options.
        /// </summary>
        [ObservableProperty]
        private string _exportMode = "BBCode";

        /// <summary>
        /// Represents the formatting options for exporting plain-text data.
        /// 'Same-line Purchase Descriptions' only applies if Include Purchase Description is true.
        /// </summary>
        [ObservableProperty]
        private List<ExportFormatToggle> _genericFormattingOptions = new()
        {
            new("Include Purchase Description", true),
            new("Same-line Purchase Descriptions", false),
            new("Include Currency Abbreviation", true),
            new("Include Section Separator", true),
            new("Include Origin Descriptions", true)
        };

        /// <summary>
        /// Represents the formatting options for exporting BBCode-formatted data.
        /// 'Apply Section formatting to purchase names' will only be used if Spoiler Purchases is set to false.
        /// 'Same-line Purchase Descriptions' only applies if Spoiler Purchases is set to false and Include Purchase Description is true.
        /// </summary>
        [ObservableProperty]
        private List<ExportFormatToggle> _bBCodeFormattingOptions = new()
        {
            new("Bold Section Names", true),
            new("Italics Section Names", false),
            new("Underline Section Names", true),
            new("Center Section Names", true),
            new("Spoiler Purchases", true),
            new("Apply Section Formatting To Purchase Names", false),
            new("Include Purchase Description", true),
            new("Same-line Purchase Descriptions", false),
            new("Include Currency Abbreviation", true),
            new("Spoiler Full Build", true),
            new("Include Section Separator", true),
            new("Section Separator Line", true),
            new("Include Origin Descriptions", true),
            new("Apply Section Formatting To Origin Details", true)
        };

        /// <summary>
        /// Represents the formatting options for exporting Markdown-formatted data.
        /// 'Same-line Purchase Descriptions' only applies if List All Purchases In One Line is set to false and Include Purchase Description is true.
        /// 'Include Purchase Description' only applies if List All Purchases In One Line is set to false.
        /// </summary>
        [ObservableProperty]
        private List<ExportFormatToggle> _markdownFormattingOptions = new()
        {
            new("Bold Section Names", true),
            new("Italics Section Names", false),
            new("Make Section Names Headings", true),
            new("Apply Section Formatting To Purchase Names", false),
            new("List All Purchases In One Line", true),
            new("Include Purchase Description", true),
            new("Same-line Purchase Descriptions", false),
            new("Include Currency Abbreviation", true),
            new("Include Section Separator", true),
            new("Section Separator Line", true),
            new("Include Origin Descriptions", true),
            new("Apply Section Formatting To Origin Details", true)
        };
    }
}
