using System.Text.Json;
using System.Text.Json.Serialization;
using Skopka.Abstraction.Serialization.Json;

namespace Skopka.Abstraction.Serialization.Tests.Json;

public class OptionalJsonConverterTests
{
    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            WriteIndented = false
        };

        options.AddOptionalSupport();

        return options;
    }

    private sealed class PatchDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<string> Name { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<int?> Age { get; init; }
    }

    [Fact]
    public void Deserialize_MissingProperty_BecomesMissing()
    {
        const string json = "{}";
        var dto = JsonSerializer.Deserialize<PatchDto>(json, CreateOptions())!;

        Assert.Equal(OptionalState.Missing, dto.Name.State);
        Assert.False(dto.Name.IsSpecified);

        Assert.Equal(OptionalState.Missing, dto.Age.State);
        Assert.False(dto.Age.IsSpecified);
    }

    [Fact]
    public void Deserialize_NullProperty_BecomesNullState()
    {
        const string json = """{"Name":null,"Age":null}""";
        var dto = JsonSerializer.Deserialize<PatchDto>(json, CreateOptions())!;

        Assert.True(dto.Name.IsSpecified);
        Assert.True(dto.Name.IsNull);
        Assert.False(dto.Name.HasValue);

        Assert.True(dto.Age.IsSpecified);
        Assert.True(dto.Age.IsNull);
        Assert.False(dto.Age.HasValue);
    }

    [Fact]
    public void Deserialize_ValueProperty_BecomesValueState()
    {
        const string json = """{"Name":"John","Age":42}""";
        var dto = JsonSerializer.Deserialize<PatchDto>(json, CreateOptions())!;

        Assert.True(dto.Name.HasValue);
        Assert.Equal("John", dto.Name.Value);

        Assert.True(dto.Age.HasValue);
        Assert.Equal(42, dto.Age.Value);
    }

    [Fact]
    public void Serialize_MissingProperties_AreOmitted()
    {
        var dto = new PatchDto
        {
            Name = default,
            Age = default
        };

        var json = JsonSerializer.Serialize(dto, CreateOptions());
        
        Assert.Equal("{}", json);
    }

    [Fact]
    public void Serialize_NullState_Properties_AreWrittenAsNull()
    {
        var dto = new PatchDto
        {
            Name = Optional<string>.FromNull(),
            Age = Optional<int?>.FromNull()
        };

        var json = JsonSerializer.Serialize(dto, CreateOptions());

        Assert.Contains("""
                        "Name":null
                        """, json);
        Assert.Contains("""
                        "Age":null
                        """, json);
        Assert.StartsWith("{", json);
        Assert.EndsWith("}", json);
    }

    [Fact]
    public void Serialize_ValueState_Properties_AreWrittenWithValues()
    {
        var dto = new PatchDto
        {
            Name = Optional<string>.From("John"),
            Age = Optional<int?>.From(42)
        };

        var json = JsonSerializer.Serialize(dto, CreateOptions());

        Assert.Contains("""
                        "Name":"John"
                        """, json);
        Assert.Contains("""
                        "Age":42
                        """, json);
    }

    [Fact]
    public void Serialize_RootMissingOptional_WritesNull()
    {
        Optional<int> opt = default;

        var json = JsonSerializer.Serialize(opt, CreateOptions());

        Assert.Equal("null", json);
    }

    [Fact]
    public void Deserialize_RootNullOptional_BecomesNullState()
    {
        const string json = "null";

        var opt = JsonSerializer.Deserialize<Optional<int>>(json, CreateOptions());

        Assert.Equal(OptionalState.Null, opt.State);
        Assert.True(opt.IsNull);
        Assert.False(opt.HasValue);
    }

    [Fact]
    public void Deserialize_RootValueOptional_BecomesValueState()
    {
        const string json = "123";

        var opt = JsonSerializer.Deserialize<Optional<int>>(json, CreateOptions());

        Assert.Equal(OptionalState.Value, opt.State);
        Assert.True(opt.HasValue);
        Assert.Equal(123, opt.Value);
    }
}