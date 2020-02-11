using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Data.Base
{
    public class BaseBundleDataModel<T> where T : class
    {
        public List<T> Data { get; set; }

        public Dictionary<string, PaginationInfoDataModel> Meta { get; set; }
    }
}
