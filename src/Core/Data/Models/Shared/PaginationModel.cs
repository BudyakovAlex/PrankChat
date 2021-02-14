using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Data.Shared
{
    public class Pagination<T>
    {
        public Pagination()
            : this(new T[0], 0)
        {
        }

        public Pagination(IReadOnlyList<T> items)
            : this(items, items?.Count ?? 0)
        {
        }

        public Pagination(IReadOnlyList<T> items, long totalCount)
        {
            Items = items ?? new List<T>();
            TotalCount = totalCount;
        }

        public IReadOnlyList<T> Items { get; }

        public long ItemsCount => Items.Count;

        public long TotalCount { get; }

        public bool IsEmpty => Items.Count == 0;
    }
}