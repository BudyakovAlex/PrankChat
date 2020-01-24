﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class OrderApiModel
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

        [JsonProperty("active_to")]
        public DateTime? ActiveTo { get; set; }

        [JsonProperty("auto_prolongation")]
        public bool? AutoProlongation { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("taken_to_work_at")]
        public DateTime? TakenToWorkAt { get; set; }

        [JsonProperty("customer")]
        public DataApiModel<UserApiModel> Customer { get; set; }

        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<UserApiModel>))]
        [JsonProperty("executor")]
        public DataApiModel<UserApiModel> Executor { get; set; }

        [JsonConverter(typeof(IgnoreUnexpectedArraysConverter<VideoMetadataApiModel>))]
        [JsonProperty("videos")]
        public DataApiModel<VideoMetadataApiModel> Video { get; set; }
    }
}
