using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class CreateOrderDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("active_for")]
        public int ActiveFor { get; set; }

        [JsonProperty("auto_prolongation")]
        public bool AutoProlongation { get; set; }

        [JsonProperty("is_private")]
        public bool IsHidden { get; set; }
    }
}
