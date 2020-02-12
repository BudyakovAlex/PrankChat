using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Api.Base
{
    public class BaseBundleApiModel<T> where T : class
    {
        public List<T> Data { get; set; }

        public Dictionary<string, PaginationInfoApiModel> Meta { get; set; }
    }
}
