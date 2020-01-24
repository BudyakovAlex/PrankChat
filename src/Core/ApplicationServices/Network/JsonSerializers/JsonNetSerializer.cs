using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers.Converters;
using RestSharp;
using RestSharp.Serialization;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.JsonSerializers
{
    public class JsonNetSerializer : IRestSerializer
    {
        private JsonSerializerSettings _settings;

        public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj);

        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content, _settings);

        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value);

        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;

        public JsonNetSerializer()
        {
            var resolver = new DefaultContractResolver();
            _settings = new JsonSerializerSettings
            {
                ContractResolver = resolver,
                Converters = { new IgnoreUnexpectedArraysConverter(resolver) },
            };
        }
    }
}
