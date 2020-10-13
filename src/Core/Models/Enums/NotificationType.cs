using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers.Converters;

namespace PrankChat.Mobile.Core.Models.Enums
{
    [JsonConverter(typeof(StringEnumJsonConverter))]
    public enum NotificationType
    {
        Unknown,

        [EnumMember(Value = "order_event")]
        OrderEvent,

        [EnumMember(Value = "wallet_event")]
        WalletEvent,

        [EnumMember(Value = "subscription_event")]
        SubscriptionEvent,

        [EnumMember(Value = "like_event")]
        LikeEvent,

        [EnumMember(Value = "comment_event")]
        CommentEvent,

        [EnumMember(Value = "executor_event")]
        ExecutorEvent,

        [EnumMember(Value = "info_event")]
        InfoEvent,
    }
}
