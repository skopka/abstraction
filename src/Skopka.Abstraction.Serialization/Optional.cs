using System.Diagnostics.CodeAnalysis;

namespace Skopka.Abstraction.Serialization;

public readonly struct Optional<T>
{
    private readonly T? _value;

    public OptionalState State { get; }

    public bool IsSpecified => State != OptionalState.Missing;
    public bool IsNull => State == OptionalState.Null;
    public bool HasValue => State == OptionalState.Value;

    public T Value => HasValue
        ? _value!
        : throw new InvalidOperationException("Value is not set (Missing or Null).");

    public static Optional<T> Missing => default;

    public static Optional<T> FromNull() => new(OptionalState.Null, default);

    public static Optional<T> From(T? value) => value is null ? FromNull() : new Optional<T>(OptionalState.Value, value);

    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        if (HasValue)
        {
            value = _value!;
            return true;
        }

        value = default;
        return false;
    }

    private Optional(OptionalState state, T? value)
    {
        State = state;
        _value = value;
    }

    public override string ToString()
        => State switch
        {
            OptionalState.Missing => "<missing>",
            OptionalState.Null => "null",
            _ => _value?.ToString() ?? "null"
        };
}