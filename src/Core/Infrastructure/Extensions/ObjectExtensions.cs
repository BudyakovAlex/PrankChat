using System;
using System.Runtime.CompilerServices;

namespace PrankChat.Mobile.Core.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static void ThrowIfNull(this object obj, [CallerMemberName] string memberName = "")
        {
            if (obj != null)
            {
                return;
            }

            throw new ArgumentNullException($"Value is null for {memberName}");
        }
    }
}
