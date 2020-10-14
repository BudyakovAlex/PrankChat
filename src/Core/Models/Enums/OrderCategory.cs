using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers.Converters;
using System.Runtime.Serialization;

namespace PrankChat.Mobile.Core.Models.Enums
{
    [JsonConverter(typeof(StringEnumJsonConverter))]
    public enum OrderCategory
    {
        Unknown,
        [EnumMember(Value = "standard")]
        Standard,
        [EnumMember(Value = "competition")]
        Competition
    }
}