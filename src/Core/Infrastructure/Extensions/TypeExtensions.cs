using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsNullableType(this Type type)
        {
            if (type.IsGenericType())
            {
                return type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            return false;
        }

        public static bool IsNullable<T>(T t) { return false; }
        public static bool IsNullable<T>(T? t) where T : struct { return true; }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static bool TryExtractValues<T>(this Type type, out T[] values)
        {
            if (type == typeof(T))
            {
                values = Enum.GetValues(type).OfType<T>().ToArray();
                return true;
            }

            values = Array.Empty<T>();
            return false;
        }

        public static IEnumerable<T> Split<T>(this Enum obj, int value)
        {
            foreach (var cur in Enum.GetValues(typeof(T)))
            {
                var number = (int)(object)(T)cur;
                if (0 != (number & value))
                {
                    yield return (T)cur;
                }
            }
        }

        public static T FirstOrValue<T>(this IEnumerable<T> source, Func<T, bool> predicate, T value)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            foreach (T local in source)
            {
                if (predicate(local))
                {
                    return local;
                }
            }

            return value;
        }
    }
}
