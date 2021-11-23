using Newtonsoft.Json;
using PrankChat.Mobile.Core.Services.Network.JsonSerializers.Converters;
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
        Competition,
        [EnumMember(Value = "private")]
        Private,
        [EnumMember(Value = "paid_competition")]
        PaidCompetition,
        [EnumMember(Value = "private_paid_competition")]
        PrivatePaidCompetition
    }
}