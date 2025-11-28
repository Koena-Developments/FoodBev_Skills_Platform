using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoodBev.API.Converters
{
    /// <summary>
    /// A JSON converter that handles enum parsing case-insensitively.
    /// This allows the frontend to send enum values in any case (e.g., "shortlisted", "Shortlisted", "SHORTLISTED").
    /// </summary>
    public class CaseInsensitiveEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string? enumString = reader.GetString();
                if (string.IsNullOrEmpty(enumString))
                {
                    throw new JsonException($"Cannot convert null or empty string to {typeof(T).Name}.");
                }
                if (Enum.TryParse<T>(enumString, ignoreCase: true, out T result))
                {
                    return result;
                }
                throw new JsonException($"Unable to convert \"{enumString}\" to {typeof(T).Name}.");
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                // Handle numeric enum values
                int enumValue = reader.GetInt32();
                if (Enum.IsDefined(typeof(T), enumValue))
                {
                    return (T)Enum.ToObject(typeof(T), enumValue);
                }
                throw new JsonException($"Unable to convert {enumValue} to {typeof(T).Name}.");
            }
            
            throw new JsonException($"Unexpected token type: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // Write enum as string using the exact enum name (PascalCase)
            writer.WriteStringValue(value.ToString());
        }
    }

    /// <summary>
    /// Factory for creating case-insensitive enum converters.
    /// </summary>
    public class CaseInsensitiveEnumConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type converterType = typeof(CaseInsensitiveEnumConverter<>).MakeGenericType(typeToConvert);
            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }
}

