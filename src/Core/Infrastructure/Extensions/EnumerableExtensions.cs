using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this)
            {
                action(item);
            }
        }

        public static int IndexOfOrDefault<T>(this T[] array, T item, int @default = 0)
        {
            var index = Array.IndexOf(array, item);
            return index < 0 ? @default : index;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return items == null || !items.Any();
        }

        public static bool IsNotNullNorEmpty<T>(this IEnumerable<T> items)
        {
            return items != null && items.Any();
        }

        public static bool IsEmpty(this IEnumerable collection)
        {
            var enumerator = collection.GetEnumerator();
            var isEmpty = !enumerator.MoveNext();
            return isEmpty;
        }

        public static bool IsNotEmpty(this IEnumerable collection)
        {
            return !IsEmpty(collection);
        }

        public static bool IsEmpty(this ICollection collection)
        {
            return collection.Count == 0;
        }

        public static bool IsNotEmpty(this ICollection collection)
        {
            return !IsEmpty(collection);
        }

        public static bool IsNullOrEmpty(this IEnumerable collection)
        {
            return collection is null || IsEmpty(collection);
        }

        public static bool IsNotNullNorEmpty(this IEnumerable collection)
        {
            return !IsNullOrEmpty(collection);
        }

        public static bool IsNullOrEmpty(this ICollection collection)
        {
            return collection is null || IsEmpty(collection);
        }

        public static bool IsNotNullNorEmpty(this ICollection collection)
        {
            return !IsNullOrEmpty(collection);
        }

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
