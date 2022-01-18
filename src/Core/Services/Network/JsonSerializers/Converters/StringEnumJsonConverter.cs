﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PrankChat.Mobile.Core.Extensions;
using System;

namespace PrankChat.Mobile.Core.Services.Network.JsonSerializers.Converters
{
    public class StringEnumJsonConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var tokenType = reader.TokenType;
            if (tokenType == JsonToken.Null && objectType.IsNullableType())
            {
                return null;
            }

            if (tokenType == JsonToken.String)
            {
                var enumText = reader.Value.ToString();
                if (enumText == string.Empty && objectType.IsNullableType())
                {
                    return null;
                }

                try
                {
                    return base.ReadJson(reader, objectType, existingValue, serializer);
                }
                catch (ArgumentException)
                {
                    // Returns default value
                    return Activator.CreateInstance(objectType);
                }
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
                var jsonSerializer = new JsonSerializer();
                jsonSerializer.Converters.Add(this);
                return jsonSerializer.Deserialize(reader, objectType);
            }

            throw new JsonSerializationException(
                string.Format(
                    "Error converting value {0} to type '{1}'.",
                    reader.Value,
                    objectType));
        }

        public override bool CanConvert(Type objectType)
        {
            var subType = objectType.IsNullableType()
                ? Nullable.GetUnderlyingType(objectType)
                : objectType;

            return subType.IsEnum();
        }
    }
}