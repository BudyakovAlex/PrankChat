using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PrankChat.Mobile.Core.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatusType
    {
        None,
        [EnumMember(Value = "new")]
        New,
        [EnumMember(Value = "rejected")]
        Rejected,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "active")]
        Active,
        [EnumMember(Value = "in_work")]
        InWork,
        [EnumMember(Value = "in_Arbitration")]
        InArbitration,
        [EnumMember(Value = "process_close_arbitration")]
        ProcessCloseArbitration,
        [EnumMember(Value = "closed_after_arbitration_customer_win")]
        ClosedAfterArbitrationCustomerWin,
        [EnumMember(Value = "closed_after_arbitration_executor_win")]
        ClosedAfterArbitrationExecutorWin,
        [EnumMember(Value = "finished")]
        Finished,
    }
}
