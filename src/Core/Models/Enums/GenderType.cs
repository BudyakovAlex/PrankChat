using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace PrankChat.Mobile.Core.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
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