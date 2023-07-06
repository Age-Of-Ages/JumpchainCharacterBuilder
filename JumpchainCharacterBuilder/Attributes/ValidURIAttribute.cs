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
            bool result = false;

            if (Uri.IsWellFormedUriString((string?)value, UriKind.Absolute) && Uri.TryCreate((string?)value, UriKind.Absolute, out Uri? uriResult) &&
                                                                                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                result = true;
            }

            return result;
        }
    }
}
