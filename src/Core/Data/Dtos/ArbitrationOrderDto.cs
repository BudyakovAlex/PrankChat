using System;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class ArbitrationOrderDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public long? Price { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public OrderStatusType? Status { get; set; }

        [JsonProperty("auto_prolongation")]
        public bool? AutoProlongation { get; set; }

        [JsonProperty("customer")]
        public ResponseDto<UserDto> Customer { get; set; }

        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<UserDto>))]
        [JsonProperty("executor")]
        public ResponseDto<UserDto> Executor { get; set; }

        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<VideoDto>))]
        [JsonProperty("videos")]
        public ResponseDto<VideoDto> Video { get; set; }

        [JsonProperty("positive_arbitration_values_count")]
        public int Likes { get; set; }

        [JsonProperty("negative_arbitration_values_count")]
        public int Dislikes { get; set; }

        [JsonProperty("arbitration_finish_at")]
        public DateTime? ArbitrationFinishAt { get; set; }
    }
}
