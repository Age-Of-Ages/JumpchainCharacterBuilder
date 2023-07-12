using System;
using System.ComponentModel.DataAnnotations;

namespace JumpchainCharacterBuilder.Attributes
{
    public sealed class ValidURIAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            bool result = false;

            if (Uri.IsWellFormedUriString((string?)value, UriKind.Absolute) && ((string?)value == "about:Blank" ||
                                                                                Uri.TryCreate((string?)value, UriKind.Absolute, out Uri? uriResult) &&
                                                                                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
            {
                result = true;
            }

            return result;
        }
    }
}
