using JumpchainCharacterBuilder.Model;
using System.Globalization;
using System.Text.RegularExpressions;

namespace JumpchainCharacterBuilder
{
    public static partial class FormatHelper
    {
        /// <summary>
        /// Copying from PDF files can cause issues with line-breaks being inserted in incorrect places, so these need to be removed.
        /// </summary>
        /// <param name="input">The string to format.</param>
        /// <param name="removeAllLineBreaks">Whether to remove every single line break. Overrides <paramref name="removeAllLineBreaks"/></param>
        /// <param name="leaveDoubleLineBreaks">Whether to leave double line breaks in the string.</param>
        /// <returns>The formatted string.</returns>
        public static string RemoveLineBreaks(string input, bool removeAllLineBreaks, bool leaveDoubleLineBreaks)
        {
            string output;

            if (removeAllLineBreaks)
            {
                output = RemoveAllLineBreaksRegex().Replace(input, " ");
            }
            else if (leaveDoubleLineBreaks)
            {
                output = RemoveLineBreaksNoDoubleRegex().Replace(input, " ");
            }
            else
            {
                output = RemoveLineBreaksRegex().Replace(input, " ");
            }

            return output;
        }

        /// <summary>
        /// If a string with too many line-breaks is put through this formatter multiple times to fully correct it then extra spaces will appear.
        /// These should be removed to preserve the tidiness of the string.
        /// Intended line-breaks also leave spaces at the beginning of the paragraph, so these should be corrected.
        /// </summary>
        /// <param name="input">The string to format.</param>
        /// <returns>The formatted string.</returns>
        public static string RemoveSpaces(string input)
        {
            string output;

            output = RemoveExcessSpacesRegex().Replace(input, " ");
            output = RemoveParagraphStartSpacesRegex().Replace(output, "");

            return output;
        }

        /// <summary>
        /// Removes any xml-unsafe characters from the string.
        /// </summary>
        /// <param name="input">The string to format.</param>
        /// <returns>The formatted string.</returns>
        public static string XmlSafeFormat(string input)
        {
            return XmlFilterRegex().Replace(input, "");
        }

        public static string FormatBudgetString(AppSettingsModel.ThousandsSeparatorFormats thousandsSeparatorFormat, int budget)
        {
            NumberFormatInfo numberFormatInfo = thousandsSeparatorFormat switch
            {
                AppSettingsModel.ThousandsSeparatorFormats.None => new()
                {
                    NumberGroupSeparator = ""
                },
                AppSettingsModel.ThousandsSeparatorFormats.Comma => new()
                {
                    NumberGroupSeparator = ","
                },
                AppSettingsModel.ThousandsSeparatorFormats.Period => new()
                {
                    NumberGroupSeparator = "."
                },
                AppSettingsModel.ThousandsSeparatorFormats.Space => new()
                {
                    NumberGroupSeparator = " "
                },
                _ => new()
                {
                    NumberGroupSeparator = ""
                },
            };
            return string.Format(numberFormatInfo, "{0:#,##0}", budget);
        }

        [GeneratedRegex("(\\r\\n)(?!\\r\\n)")]
        private static partial Regex RemoveLineBreaksRegex();

        [GeneratedRegex("(\\r\\n)")]
        private static partial Regex RemoveAllLineBreaksRegex();

        [GeneratedRegex("(?<!\\r\\n)(\\r\\n)(?!\\r\\n)")]
        private static partial Regex RemoveLineBreaksNoDoubleRegex();

        [GeneratedRegex(@"\s{2,}")]
        private static partial Regex RemoveExcessSpacesRegex();

        [GeneratedRegex("(?<=\\n) ")]
        private static partial Regex RemoveParagraphStartSpacesRegex();

        [GeneratedRegex("[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD]", RegexOptions.Compiled)]
        private static partial Regex XmlFilterRegex();
    }
}
