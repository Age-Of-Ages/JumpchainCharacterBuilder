using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder.Attributes
{
    public sealed partial class XmlFilterAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Regex filterRegex = FilterRegex();

            if (value != null && !filterRegex.IsMatch((string)value))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new("Textbox contains invalid characters, please use the Input Formatter to remove them, then select all text in this box with ctrl-a before pasting.");
            }
        }

        [GeneratedRegex("[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD]", RegexOptions.Compiled)]
        private static partial Regex FilterRegex();
    }
}
