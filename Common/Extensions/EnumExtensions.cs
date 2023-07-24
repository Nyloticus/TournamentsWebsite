using System;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
        public static bool TryParseEnum<TEnum>(this int enumValue, out TEnum retVal)
        {
            retVal = default(TEnum);
            bool success = Enum.IsDefined(typeof(TEnum), enumValue);
            if (success)
            {
                retVal = (TEnum)Enum.ToObject(typeof(TEnum), enumValue);
            }
            return success;
        }

        public static T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("T must be an Enumeration type.");
            }
            if (Enum.TryParse(str, true, out T val))
            {
                return val;
            }
            else
            {
                throw new ArgumentException($"Invalid value '{str}' for Enum type '{enumType.FullName}'");
            }
        }

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("T must be an Enumeration type.");
            }
            try
            {
                return (T)Enum.ToObject(enumType, intValue);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid value '{intValue}' for Enum type '{enumType.FullName}'", ex);
            }
        }
    }
}