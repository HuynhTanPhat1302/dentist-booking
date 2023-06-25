using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public class MaxDepthJsonConverter : JsonConverter<object>
{
    private readonly int maxDepth;

    public MaxDepthJsonConverter(int maxDepth)
    {
        this.maxDepth = maxDepth;
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return true;
    }

    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Implement deserialization logic if needed
        return JsonSerializer.Deserialize(ref reader, typeToConvert, options);
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        WriteValue(writer, value, options, 0);
    }

    private void WriteValue(Utf8JsonWriter writer, object value, JsonSerializerOptions options, int currentDepth)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        var valueType = value.GetType();
        var properties = valueType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead && !p.GetIndexParameters().Any())
            .ToList();

        writer.WriteStartObject();

        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(value);

            if (currentDepth < maxDepth)
            {
                writer.WritePropertyName(property.Name);
                WriteValue(writer, propertyValue, options, currentDepth + 1);
            }
            else
            {
                if (propertyValue is IEnumerable enumerable)
                {
                    writer.WritePropertyName(property.Name);
                    writer.WriteStartArray();

                    foreach (var item in enumerable)
                    {
                        writer.WriteNullValue();
                    }

                    writer.WriteEndArray();
                }
                else
                {
                    writer.WriteNull(property.Name);
                }
            }
        }

        writer.WriteEndObject();
    }
}
