using System.Reflection;

namespace JumpchainCharacterBuilder
{
    public static class PropertyAccess
    {
        public static object? GetProperty(this object? target, string propertyName)
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

        public static string? GetString(this object target, string propertyName)
        {
            object? value = GetProperty(target, propertyName);

            if (value is string output)
            {
                return output;
            }
            return null;
        }
    }
}
