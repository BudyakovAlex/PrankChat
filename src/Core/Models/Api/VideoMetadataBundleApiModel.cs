using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class VideoMetadataBundleApiModel
    {
        public List<VideoMetadataApiModel> Data { get; set; }

        public Dictionary<string, PaginationInfoApiModel> Meta { get; set; }
    }
}
