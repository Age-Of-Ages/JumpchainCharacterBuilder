using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace JumpchainCharacterBuilder.Attributes
{
    public sealed class UniqueNameAttribute(string propertyName) : ValidationAttribute
    {
        private readonly string _propertyName = propertyName;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;

            PropertyInfo? listProperty = instance.GetType().GetProperty(_propertyName);

            if (listProperty != null && listProperty.GetValue(instance, null) is IEnumerable<string> comparisonList)
            {
                if (comparisonList.Where(x => x.Equals(value)).Count() > 1)
                {
                    return new("Name must be unique.");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            return new("Invalid input");
        }
    }
}
