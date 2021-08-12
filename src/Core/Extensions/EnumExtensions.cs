using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PrankChat.Mobile.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumMemberAttrValue<T>(this T enumValue)
        {
            var enumType = typeof(T);
            var memInfo = enumType.GetMember(enumValue.ToString());
            var attr = memInfo.FirstOrDefault()?
                              .GetCustomAttributes(false)
                              .OfType<EnumMemberAttribute>()
                              .FirstOrDefault();

            return attr?.Value;
        }

        public static IEnumerable<string> GetEnumMembersAttrValues<T>(this T[] enumValues)
        {
            var enumType = typeof(T);
            var values = enumValues.Select(e => enumType.GetMember(e.ToString())
                                                .FirstOrDefault()?
                                                .GetCustomAttributes(false)
                                                .OfType<EnumMemberAttribute>()
                                                .FirstOrDefault()
                                                ?.Value);
            return values;
        }

        public static T ToEnum<T>(this string enumMember) where T : struct
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).SingleOrDefault();
                if (enumMemberAttribute?.Value == enumMember)
                    return (T)Enum.Parse(enumType, name);
            }
            return default;
        }
    }
}
