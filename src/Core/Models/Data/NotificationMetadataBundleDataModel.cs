using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Data
{
    public class NotificationMetadataBundleDataModel
    {
        public List<NotificationMetadataDataModel> Data { get; set; }

        public Dictionary<string, PaginationInfoDataModel> Meta { get; set; }
    }
}
