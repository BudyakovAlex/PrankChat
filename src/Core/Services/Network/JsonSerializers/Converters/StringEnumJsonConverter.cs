using Newtonsoft.Json;
using PrankChat.Mobile.Core.Extensions;
using System;

namespace PrankChat.Mobile.Core.Services.Network.JsonSerializers.Converters
{
    public class StringEnumJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                Enum enumObject = value as Enum;
                if (enumObject != null)
                {
                    string str = enumObject.ToString("G");
                    writer.WriteValue(str);
                }
                else
                {
                    var s = new JsonSerializer();
                    s.Converters.Add(this);

                    s.Serialize(writer, value);
                }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JsonToken tokenType = reader.TokenType;

            if (tokenType == JsonToken.Null && objectType.IsNullableType())
            {
                return null;
            }

            if (tokenType == JsonToken.String)
            {
                string enumText = reader.Value.ToString();
                if (enumText == string.Empty && objectType.IsNullableType())
                {
                    return null;
                }

                if (objectType.IsNullableType())
                {
                    objectType = Nullable.GetUnderlyingType(objectType);
                }

                //enumText = enumText.Replace("_", "");

                Enum enumObject;
                try
                {
                    enumObject = (Enum)Enum.Parse(objectType, enumText, true);
                }
                catch (ArgumentException)
                {
                    // Returns default value
                    return Activator.CreateInstance(objectType);
                }

                return enumObject;
            }

            // Workaround for correct deserialization of integer values to enums
            if (tokenType == JsonToken.Integer)
            {
                if (objectType.IsNullableType())
                {
                    objectType = Nullable.GetUnderlyingType(objectType);
                }

                return Enum.ToObject(objectType, reader.Value);
            }

            // Workaround for lists of enums
            if (tokenType == JsonToken.StartArray)
            {
                JsonSerializer s = new JsonSerializer();
                s.Converters.Add(this);

                return s.Deserialize(reader, objectType);
            }

            throw new JsonSerializationException(
                string.Format(
                    "Error converting value {0} to type '{1}'.",
                    reader.Value,
                    objectType));
        }

        public override bool CanConvert(Type objectType)
        {
            Type subType = objectType.IsNullableType()
                ? Nullable.GetUnderlyingType(objectType)
                : objectType;
            return subType.IsEnum();
        }
    }
}