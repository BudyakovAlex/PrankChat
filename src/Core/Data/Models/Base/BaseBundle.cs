using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Models.Data.Base
{
    public class BaseBundle<T> where T : class
    {
        public List<T> Data { get; set; }

        public Dictionary<string, PaginationInfo> Meta { get; set; }
    }
}
