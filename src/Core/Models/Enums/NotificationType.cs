using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PrankChat.Mobile.Core.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NotificationType
    {
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
