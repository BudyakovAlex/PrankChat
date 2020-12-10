using System.Runtime.Serialization;

namespace PrankChat.Mobile.Core.Models.Enums
{
    public enum GenderType
    {
        [EnumMember(Value = "unknown")]
        Unknown,
        [EnumMember(Value = "female")]
        Female,
        [EnumMember(Value = "male")]
        Male,
    }
}