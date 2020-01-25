﻿using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    public class DataApiModel<T> where T : class, new()
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}