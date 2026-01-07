namespace Skopka.Abstraction.Serialization.Tests;

public class OptionalTests
{
    [Fact]
    public void Default_IsMissing()
    {
        Optional<int> opt = default;

        Assert.Equal(OptionalState.Missing, opt.State);
        Assert.False(opt.IsSpecified);
        Assert.False(opt.IsNull);
        Assert.False(opt.HasValue);
    }

    [Fact]
    public void FromNull_IsNull()
    {
        var opt = Optional<string>.FromNull();

        Assert.Equal(OptionalState.Null, opt.State);
        Assert.True(opt.IsSpecified);
        Assert.True(opt.IsNull);
        Assert.False(opt.HasValue);
        Assert.Throws<InvalidOperationException>(() => _ = opt.Value);
    }

    [Fact]
    public void FromValue_IsValue()
    {
        var opt = Optional<int>.From(123);

        Assert.Equal(OptionalState.Value, opt.State);
        Assert.True(opt.IsSpecified);
        Assert.False(opt.IsNull);
        Assert.True(opt.HasValue);
        Assert.Equal(123, opt.Value);
    }

    [Fact]
    public void From_Null_BecomesNullState()
    {
        var opt = Optional<string>.From(null);

        Assert.Equal(OptionalState.Null, opt.State);
        Assert.True(opt.IsNull);
        Assert.Throws<InvalidOperationException>(() => _ = opt.Value);
    }

    [Fact]
    public void Value_Throws_WhenMissing()
    {
        Optional<Guid> opt = default;

        Assert.Throws<InvalidOperationException>(() => _ = opt.Value);
    }

    [Fact]
    public void TryGetValue_ReturnsFalse_ForMissing()
    {
        Optional<int> opt = default;

        var ok = opt.TryGetValue(out var value);

        Assert.False(ok);
        Assert.Equal(0, value);
    }

    [Fact]
    public void TryGetValue_ReturnsFalse_ForNull()
    {
        var opt = Optional<string>.FromNull();

        var ok = opt.TryGetValue(out var value);

        Assert.False(ok);
        Assert.Null(value);
    }

    [Fact]
    public void TryGetValue_ReturnsTrue_ForValue()
    {
        var opt = Optional<string>.From("abc");

        var ok = opt.TryGetValue(out var value);

        Assert.True(ok);
        Assert.Equal("abc", value);
    }
}