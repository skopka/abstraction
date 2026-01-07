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

- `documentation/OperationResult.md` (in the repository)
