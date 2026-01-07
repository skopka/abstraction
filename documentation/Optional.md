# Serialization: Optional

`Optional<T>` is a tri-state wrapper provided by **Skopka.Abstraction.Serialization**.

It allows you to distinguish three different cases when reading data from external formats (JSON, forms, messages, etc.):

- **Missing**: the value/property was not provided at all
- **Null**: the value/property was provided explicitly as `null`
- **Value**: the value/property was provided with a concrete value

This is especially useful for **PATCH / partial update** scenarios where “not provided” must be treated differently from “set to null”.

> Note: the package is named **Serialization**.  
> `Optional<T>` is the first functionality currently implemented in this package.

---

## Concepts

### States

`Optional<T>` has three states:

- `OptionalState.Missing`  
  The value/property is absent. This is also the default state (`default(Optional<T>)`).

- `OptionalState.Null`  
  The value/property is present and explicitly set to `null`.

- `OptionalState.Value`  
  The value/property is present and contains a value.

---

## API

Typical members:

- `State` → `OptionalState`
- `IsSpecified` → `true` for `Null` and `Value`
- `IsNull` → `true` only for `Null`
- `HasValue` → `true` only for `Value`
- `Value` → returns the stored value only when `HasValue == true`
    - throws `InvalidOperationException` for `Missing` and `Null`
- `TryGetValue(out T value)` → safe way to read the value

Factory helpers:

- `Optional<T>.Missing` (same as `default(Optional<T>)`)
- `Optional<T>.FromNull()`
- `Optional<T>.From(T? value)`
    - if `value` is `null` → returns `Null`
    - otherwise → returns `Value`

---

## Usage examples

### Basic

```c#
Optional<int> a = default;                        // Missing
Optional<string> b = Optional<string>.FromNull(); // Null
Optional<Guid> c = Optional<Guid>.From(Guid.NewGuid()); // Value

if (a.IsSpecified)
{
    // never happens for Missing
}

if (b.IsNull)
{
    // explicitly provided null
}

if (c.HasValue)
{
    var id = c.Value;
}
```

### Safe read

```c#
var opt = Optional<string>.From("hello");

if (opt.TryGetValue(out var value))
{
    Console.WriteLine(value);
}
```

---

## JSON integration (System.Text.Json)

The repository includes a JSON adapter under:

- namespace: `Skopka.Abstraction.Serialization.Json`
- converter factory: `OptionalJsonConverterFactory`

### Register converter

```c#
services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new OptionalJsonConverterFactory());
});
```

(or use a `JsonSerializerOptions` extension if you added one).

### DTO for PATCH / partial updates

To omit Missing properties during serialization, use:

`[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]`

```c#
public sealed class PatchUserRequest
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<string> Name { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int?> Age { get; init; }
}
```

Behavior:

- Property **absent** in JSON → `Optional<T>` becomes **Missing**
- Property present as `null` → `Optional<T>` becomes **Null**
- Property present with value → `Optional<T>` becomes **Value**

### Applying a patch

```c#
if (req.Name.IsSpecified)
{
    if (req.Name.IsNull)
        user.Name = null;           // explicit "clear" request
    else
        user.Name = req.Name.Value; // set new value
}
// If Missing -> do nothing
```

---

## Notes

- `Optional<T>` itself is format-agnostic.  
  A specific format (JSON, XML, form-data, etc.) must provide an adapter/converter that sets the correct state.
- `default(Optional<T>)` is always **Missing**.

---