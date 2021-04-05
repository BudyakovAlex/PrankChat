using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PrankChat.Mobile.Core.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WithdrawalStatusType
    {
        [EnumMember(Value = "new")]
        New,

        [EnumMember(Value = "approved")]
        Approved,

        [EnumMember(Value = "rejected")]
        Rejected,

        [EnumMember(Value = "finished")]
        Finished,
    }
}
