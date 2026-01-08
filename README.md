# Skopka.Abstraction

A small collection of reusable .NET abstractions and primitives.

> Current focus: **OperationResult** (success/failure + typed value + composable extensions).

[`Changelog`](CHANGELOG.md)

---

## Packages

### Skopka.Abstraction.OperationResult
A lightweight `OperationResult` / `OperationResult<T>` implementation for representing success/failure outcomes without using exceptions for flow control.

- Structured error model (`Error` with `Code`, `Message`, `Type`, optional `Details`)
- Functional composition: `Map`, `Bind`, `Match`, `Tap`, `TapError`, `Ensure`
- LINQ query syntax support (`Select`, `SelectMany`, `Where`)

Documentation:
- [`OperationResult`](documentation/OperationResult.md)

### Skopka.Abstraction.Serialization
Serialization-related primitives and helpers.

> This package is named **Serialization**.  
> Currently it provides one feature: `Optional<T>` (tri-state wrapper: Missing vs Null vs Value) plus JSON support.

- `Optional<T>`: tri-state wrapper for partial updates and “presence” tracking
- System.Text.Json integration: `Skopka.Abstraction.Serialization.Json` (`OptionalJsonConverterFactory`)

Documentation:
- [`Serialization: Optional`](documentation/Optional.md)

### Skopka.Abstraction.Data
Data helpers.

> Currently it provides Base64 / Base64Url extension methods for `byte[]`, `ReadOnlySpan<byte>` and `string`.

- Base64 encoding/decoding
- Base64Url encoding/decoding (URL-safe, without padding)

Documentation:
- [`Encoding: Base64 / Base64Url`](documentation/Base64.md)

### Skopka.Abstraction
Meta-package that references and ships **all** packages from this repository in a single dependency.

- Use it if you want “everything from Skopka.Abstraction” without adding individual packages.

---

## License

Licensed under the **Apache License 2.0**. See the `LICENSE` file.
