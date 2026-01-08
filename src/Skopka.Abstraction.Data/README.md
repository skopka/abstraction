
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

- `documentation/Base64.md` (in the repository)