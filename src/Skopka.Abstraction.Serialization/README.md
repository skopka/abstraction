# Skopka.Abstraction.Serialization

Serialization-related primitives and helpers.

Currently this package provides one feature: `Optional<T>` (tri-state wrapper: Missing / Null / Value) plus JSON integration.

## Install

```bash
dotnet add package Skopka.Abstraction.Serialization
```

## Optional<T> overview

`Optional<T>` helps you distinguish:

- Missing (property not present)
- Null (property present and set to null)
- Value (property present with value)

## Documentation

- `documentation/Optional.md` (in the repository)
