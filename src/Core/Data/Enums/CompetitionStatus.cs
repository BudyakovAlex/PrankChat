using System.Runtime.Serialization;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Services.Network.JsonSerializers.Converters;

namespace PrankChat.Mobile.Core.Models.Enums
{
    [JsonConverter(typeof(StringEnumJsonConverter))]
    public enum CompetitionStatus
    {
        [EnumMember(Value = "new")]
        New,
        [EnumMember(Value = "competition_upload_videos")]
        UploadVideos,
        [EnumMember(Value = "competition_voting")]
        Voting,
        [EnumMember(Value = "finished")]
        Finished,
        [EnumMember(Value = "moderation")]
        Moderation
    }
}