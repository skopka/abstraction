using System.Text.Json;

namespace Skopka.Abstraction.Serialization.Json;

public static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions AddOptionalSupport(this JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Converters.Add(new OptionalJsonConverterFactory());
        return options;
    }
}