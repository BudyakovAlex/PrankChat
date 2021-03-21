using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Dtos
{
    public class DocumentDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }
    }
}
