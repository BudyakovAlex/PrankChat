using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class VideoMetadataBundleDataModel
    {
        public List<VideoMetadataDataModel> Data { get; set; }

        public Dictionary<string, PaginationInfoDataModel> Meta { get; set; }
    }
}
