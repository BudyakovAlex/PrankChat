using System;
using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Models.Api
{
    /// <summary>
    /// If you want to remove or to add a field, you should add a new parameter in PostFile method in HttpClient.cs.
    /// </summary>
    public class LoadVideoApiModel
    {
        public int OrderId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Video { get; set; }

        public string FilePath { get; set; }
    }
}
