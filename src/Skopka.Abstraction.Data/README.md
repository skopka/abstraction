
# Skopka.Abstraction.Data

Small data helpers (currently: Base64 / Base64Url).

## Install

```bash
dotnet add package Skopka.Abstraction.Encoding
```

## Quick start

```c#
var bytes = Guid.NewGuid().ToByteArray();

var b64 = bytes.ToBase64();
var b64url = bytes.ToBase64Url();

var back1 = b64.FromBase64();
var back2 = b64url.FromBase64Url();
```

## Documentation

- [Source documentation](https://github.com/skopka/abstraction/blob/v0.2.0/documentation/Base64.md)

Guides and API reference cover version **0.2.0**:

- [Product overview (English)](https://products.skopka.ru/en/abstraction/index.html)
- [English guide](https://products.skopka.ru/guides/en/abstraction.html)
- [Russian guide](https://products.skopka.ru/guides/ru/abstraction.html)
- [API reference](https://products.skopka.ru/api/abstraction/index.html)
