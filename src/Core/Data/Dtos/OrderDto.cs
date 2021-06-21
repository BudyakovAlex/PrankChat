using System;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class OrderDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("type")]
        public OrderCategory? OrderCategory { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public OrderStatusType? Status { get; set; }

        [JsonProperty("active_to")]
        public DateTime? ActiveTo { get; set; }

        [JsonProperty("close_order_at")]
        public DateTime? CloseOrderAt { get; set; }

        [JsonProperty("duration")]
        public int DurationInHours { get; set; }

        [JsonProperty("auto_prolongation")]
        public bool? AutoProlongation { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("taken_to_work_at")]
        public DateTime? TakenToWorkAt { get; set; }

        [JsonProperty("video_uploaded_at")]
        public DateTime? VideoUploadedAt { get; set; }

        [JsonProperty("my_arbitration_value")]
        public ArbitrationValueType? MyArbitrationValue { get; set; }

        [JsonProperty("arbitration_finish_at")]
        public DateTime? ArbitrationFinishAt { get; set; }

        [JsonProperty("negative_arbitration_values_count")]
        public int? NegativeArbitrationValuesCount { get; set; }

        [JsonProperty("positive_arbitration_values_count")]
        public int? PositiveArbitrationValuesCount { get; set; }

        [JsonProperty("customer")]
        public ResponseDto<UserDto> Customer { get; set; }

        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<UserDto>))]
        [JsonProperty("executor")]
        public ResponseDto<UserDto> Executor { get; set; }

        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<VideoDto>))]
        [JsonProperty("videos")]
        public ResponseDto<VideoDto> Video { get; set; }
    }
}
