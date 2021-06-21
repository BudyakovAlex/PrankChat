using System;
using System.Runtime.Serialization;

namespace PrankChat.Mobile.Core.Data.Enums
{
    [Flags]
    public enum IncludeType
    {
        [EnumMember(Value = "user")]
        User,

        [EnumMember(Value = "comments")]
        Comments,

        [EnumMember(Value = "customer")]
        Customer,

        [EnumMember(Value = "executor")]
        Executor,

        [EnumMember(Value = "videos")]
        Videos,

        [EnumMember(Value = "arbitration_values")]
        ArbitrationValues,

        [EnumMember(Value = "order")]
        Order,

        [EnumMember(Value = "document")]
        Document,
    }
}
