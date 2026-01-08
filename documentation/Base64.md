# Encoding: Base64 / Base64Url

This package provides small, allocation-friendly helpers for Base64 and Base64Url encoding/decoding.

It includes:
- `byte[]` and `ReadOnlySpan<byte>` encoding to Base64 / Base64Url
- `string` decoding from Base64 / Base64Url to `byte[]`

Base64Url is the URL-safe variant of Base64:
- `+` is replaced with `-`
- `/` is replaced with `_`
- padding `=` is removed

---

## Install

```bash
dotnet add package Skopka.Abstraction.Encoding
```

---

## API

### Encoding

```c#
byte[] bytes = ...;

string base64 = bytes.ToBase64();
string base64Url = bytes.ToBase64Url();
```

Span overload:

```c#
ReadOnlySpan<byte> span = bytes.AsSpan();

string base64 = span.ToBase64();
string base64Url = span.ToBase64Url();
```

### Decoding

```c#
byte[] bytes1 = "AQIDBAU=".FromBase64();
byte[] bytes2 = "AQIDBAU".FromBase64Url();
```

`FromBase64Url()` automatically restores padding (`=`) when needed.

---

## Typical usage (PATCH / tokens / URLs)

Base64Url is often used for:
- tokens
- URLs and query strings
- identifiers that must be URL-safe

```c#
  var idBytes = Guid.NewGuid().ToByteArray();
  var urlSafe = idBytes.ToBase64Url();

  // later
  var decoded = urlSafe.FromBase64Url();
```

---

## Notes

- Accessing invalid input throws `FormatException` (same as `Convert.FromBase64String`).
- `ToBase64Url()` produces output without `+`, `/`, `=`.
