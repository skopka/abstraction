# Skopka.Abstraction.OperationResult

Lightweight `OperationResult` / `OperationResult<T>` primitives for representing success/failure outcomes with structured errors and composable extensions.

## Install

```bash
dotnet add package Skopka.Abstraction.OperationResult
```

## Quick start

```c#
OperationResult<Guid> CreateUser(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        return OperationResultFactory.Fail<Guid>(
            new Error(ErrorCodes.ValidationRequired, "Name is required", ErrorType.Validation));

    return OperationResultFactory.Success(Guid.NewGuid());
}

var id = CreateUser("John")
    .Tap(x => Console.WriteLine($"Created: {x}"))
    .Match(ok => ok, fail => Guid.Empty);
```

## Documentation

- [Source documentation](https://github.com/skopka/abstraction/blob/v0.2.0/documentation/OperationResult.md)

Guides and API reference cover version **0.2.0**:

- [Product overview (English)](https://products.skopka.ru/en/abstraction/index.html)
- [English guide](https://products.skopka.ru/guides/en/abstraction.html)
- [Russian guide](https://products.skopka.ru/guides/ru/abstraction.html)
- [API reference](https://products.skopka.ru/api/abstraction/index.html)
