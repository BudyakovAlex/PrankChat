using System.Runtime.Serialization;

namespace PrankChat.Mobile.Core.Models.Data
{
    public enum DateFilterType
    {
        [EnumMember(Value = "day")]
        Day,
        [EnumMember(Value = "week")]
        Week,
        [EnumMember(Value = "month")]
        Month,
        [EnumMember(Value = "quarter")]
        Quarter,
        [EnumMember(Value = "half_year")]
        HalfYear,
        [EnumMember(Value = "year")]
        Year,
        [EnumMember(Value = "all")]
        All
    }
}
