using System;
namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static void ThrowIfNull(this object obj, string name)
        {
            if (obj != null)
            {
                return;
            }

            throw new ArgumentNullException($"Value is null for {name}");
        }
    }
}
