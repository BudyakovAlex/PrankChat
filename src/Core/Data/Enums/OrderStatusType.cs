﻿using System.Runtime.Serialization;
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

        [EnumMember(Value = "in_arbitration")]
        InArbitration,

        [EnumMember(Value = "process_close_arbitration")]
        ProcessCloseArbitration,

        [EnumMember(Value = "closed_after_arbitration_customer_win")]
        ClosedAfterArbitrationCustomerWin,

        [EnumMember(Value = "closed_after_arbitration_executor_win")]
        ClosedAfterArbitrationExecutorWin,

        [EnumMember(Value = "video_in_process")]
        VideoInProcess,

        [EnumMember(Value = "video_wait_moderation")]
        VideoWaitModeration,

        [EnumMember(Value = "process_error")]
        VideoProcessError,

        [EnumMember(Value = "finished")]
        Finished,

        [EnumMember(Value = "wait_finish")]
        WaitFinish,

        [EnumMember(Value = "competition_voting")]
        CompetitionVoting,

        [EnumMember(Value = "competition_upload_videos")]
        competitionUploadVideos,
    }
}
