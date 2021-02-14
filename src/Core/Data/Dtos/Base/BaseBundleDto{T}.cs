using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Data.Dtos.Base
{
    public class BaseBundleDto<T> where T : class
    {
        public List<T> Data { get; set; }

        public Dictionary<string, PaginationInfoDto> Meta { get; set; }
    }
}
