using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace FitCode.Identity.Extensions
{
    public static class ConvertExtensions
    {
        public static T Get<T>(this string value, T defaultValue = default(T))
        {
            TypeConverter converter;
            Type newType = null;
            Type nullableCreatedType = null;
            if (value == null && typeof(T).IsValueType)
            {
                newType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                nullableCreatedType = typeof(Nullable<>).MakeGenericType(newType);
                converter = new NullableConverter(nullableCreatedType);
            }
            else
            {
                converter = TypeDescriptor.GetConverter(typeof(T));
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)Convert.ChangeType(Constants.TrueValues.Any(a => a.Equals(value, StringComparison.OrdinalIgnoreCase)), TypeCode.Boolean);
            }

            if (converter.CanConvertFrom(typeof(string)) == false)
            {
                return defaultValue;
            }

            var answer = converter.ConvertFromInvariantString(value);
            if (answer == null && newType != null)
            {
                return (T)Activator.CreateInstance(nullableCreatedType);
            }

            return (T)converter.ConvertFromInvariantString(value);
        }

        public static T GetFromCollection<T>(this NameValueCollection nameValueCollection, string key, T defaultValue = default(T))
        {
            if (nameValueCollection == null || nameValueCollection.Count == 0) return defaultValue;

            var setting = nameValueCollection[key];
            if (!string.IsNullOrWhiteSpace(setting))
            {
                return setting.Get<T>(defaultValue);
            }

            return defaultValue;
        }
    }
}
