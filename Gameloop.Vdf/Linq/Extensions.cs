using Gameloop.Vdf.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Gameloop.Vdf.Linq
{
    public static class Extensions
    {
        public static U Value<U>(this IEnumerable<VToken> value)
        {
            return value.Value<VToken, U>();
        }

        public static U Value<T, U>(this IEnumerable<T> value) where T : VToken
        {
            ValidationUtils.ArgumentNotNull(value, nameof(value));

            if (!(value is VToken token))
                throw new ArgumentException("Source value must be a JToken.");

            return token.Convert<VToken, U>();
        }

        internal static U Convert<T, U>(this T token) where T : VToken
        {
            if (token == null)
                return default(U);

            if (token is U
                // don't want to cast JValue to its interfaces, want to get the internal value
                && typeof(U) != typeof(IComparable) && typeof(U) != typeof(IFormattable))
            {
                // HACK
                return (U) (object) token;
            }
            else
            {
                VValue value = token as VValue;
                if (value == null)
                    throw new InvalidCastException($"Cannot cast {token.GetType()} to {typeof(T)}.");

                if (value.Value is U u)
                    return u;

                Type targetType = typeof(U);

                if (ReflectionUtils.IsNullableType(targetType))
                {
                    if (value.Value == null)
                        return default(U);

                    targetType = Nullable.GetUnderlyingType(targetType);
                }

                if (TryConvertVdf<U>(value.Value, out U resultObj))
                    return resultObj;

                return (U) System.Convert.ChangeType(value.Value, targetType, CultureInfo.InvariantCulture);
            }
        }

        private static bool TryConvertVdf<U>(object value, out U result)
        {
            result = default(U);

            // It won't be null at this point, so just handle the nullable type.
            if ((typeof(U) == typeof(bool) || Nullable.GetUnderlyingType(typeof(U)) == typeof(bool)) && value is string valueString)
            {
                switch (valueString)
                {
                    case "1":
                        result = (U) (object) true;
                        return true;

                    case "0":
                        result = (U) (object) false;
                        return true;
                }
            }

            return false;
        }
    }
}
