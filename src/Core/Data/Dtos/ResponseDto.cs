using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class ResponseDto<T> where T : class, new()
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class ResponseDto
    {
        [JsonProperty("data")]
        public object Data { get; set; }
    }
}