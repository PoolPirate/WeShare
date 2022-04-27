using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeShare.WebAPI.Converters;

public class NullableBooleanConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType switch
    {
        JsonTokenType.False => false,
        JsonTokenType.True => true,
        JsonTokenType.Number => reader.TryGetByte(out byte value) && value == 1,
        JsonTokenType.String => Boolean.TryParse(reader.GetString(), out bool val) && val,
        _ => throw new NotSupportedException()
    };

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) => writer.WriteBooleanValue(value);
}
