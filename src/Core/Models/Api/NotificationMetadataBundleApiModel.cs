using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class NotificationMetadataBundleApiModel
    {
        public List<NotificationMetadataApiModel> Data { get; set; }

        public Dictionary<string, PaginationInfoApiModel> Meta { get; set; }
    }
}
