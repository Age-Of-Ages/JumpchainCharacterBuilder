using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder
{
    public static class PropertyAccess
    {
        public static Object? GetProperty(this Object? target, string propertyName)
        {
            foreach (string subString in propertyName.Split('.'))
            {
                if (target == null)
                {
                    return null;
                }

                PropertyInfo? propertyInfo = target.GetType().GetProperty(subString);
                if (propertyInfo == null)
                {
                    return null;
                }

                target = propertyInfo.GetValue(target, null);
            }

            return target;
        }

        public static string? GetString(this Object target, string propertyName)
        {
            Object? value = GetProperty(target, propertyName);

            if (value is string output)
            {
                return output;
            }
            return null;
        }
    }
}
