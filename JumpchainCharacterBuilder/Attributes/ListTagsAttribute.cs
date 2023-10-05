using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder.Attributes
{
    public sealed partial class ListTagsAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            bool result = false;
            Regex tagCheckRegex = TagCheckRegex();

            if (value != null && !tagCheckRegex.IsMatch((string)value))
            {
                result = true;
            }

            return result;
        }

        [GeneratedRegex("((?<=^#)|[\\|\\[\\]])", RegexOptions.Compiled)]
        private static partial Regex TagCheckRegex();
    }
}
