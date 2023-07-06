using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder.Attributes
{
    public sealed class ValidURIAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            bool result = true;

            if (!Uri.IsWellFormedUriString((string?)value, UriKind.Absolute))
            {
                result = false;
            }

            return result;
        }
    }
}
