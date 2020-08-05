using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace PrankChat.Mobile.Core.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderCategory
    {
        [EnumMember(Value = "standard")]
        Standard,
        [EnumMember(Value = "competition")]
        Competition
    }
}