# Skopka.Abstraction.OperationResult

## What it is

`OperationResult` represents an operation that either **succeeds** or **fails**.

- No exceptions for expected failures (validation, not found, conflict, etc.)
- Errors are explicit and can be propagated without try/catch noise
- Results are immutable and safe to pass around

## Core types

```c#
public class OperationResult
{
    public bool IsSuccess { get; }
    public IReadOnlyCollection<Error> Errors { get; }
}

public sealed class OperationResult<T> : OperationResult
{
    public T Value => IsSuccess ? field : throw new InvalidOperationException("Value is not set");
}
```

> `Value` is **only valid when `IsSuccess == true`**.  
> Accessing `Value` on a failed result throws `InvalidOperationException`.

---

# Installation

Install the NuGet package:

    dotnet add package Skopka.Abstraction.OperationResult

---

# Error model

Errors are represented by `Error`.

Typical fields:
- `Code` — stable identifier (string)
- `Message` — human-readable message
- `Type` — error category (enum)
- `Details` — optional structured details (object)

Example codes:

```c#
public static class ErrorCodes
{
    public const string Unknown = "skopka.common.unknown";

    public const string ValidationRequired = "skopka.validation.required";
    public const string ValidationFormat = "skopka.validation.format";
    public const string ValidationPredicateFailed = "skopka.validation.predicate_failed";

    public const string NotFound = "skopka.common.not_found";
    public const string Conflict = "skopka.common.conflict";

    public const string Unauthorized = "skopka.auth.unauthorized";
    public const string Forbidden = "skopka.auth.forbidden";
}
```

> The library intentionally does **not** bind error types/codes to HTTP status codes.  
> Mapping to HTTP is usually done at the API boundary (ASP.NET Core layer).

---

# Creating results

Use the factory to create success/failure results.

```c#
// success without value
OperationResult ok = OperationResultFactory.Success();

// success with value
OperationResult<Guid> okId = OperationResultFactory.Success(Guid.NewGuid());

// fail (non-generic)
OperationResult fail = OperationResultFactory.Fail(
    new Error(ErrorCodes.Conflict, "Conflict", ErrorType.Conflict)
);

// fail (generic)
OperationResult<int> failInt = OperationResultFactory.Fail<int>(
    new Error(ErrorCodes.ValidationRequired, "Value is required", ErrorType.Validation)
);
```

---

# Composition

## Map

Transform the success value `T -> TResult`.  
If the source is failed, errors are propagated and the mapping function is **not executed**.

```c#
OperationResult<int> r = OperationResultFactory.Success(10);

OperationResult<string> mapped = r.Map(x => x.ToString());
// => Success("10")
```

## Bind

Chain operations where the next step can also fail: `T -> OperationResult<TResult>`.

```c#
OperationResult<User> user = GetUser(userId);

OperationResult<Token> token =
    user.Bind(u => IssueToken(u));
```

## Match

Collapse a result into a single value by handling both paths.

```c#
var text = result.Match(
    ok: value => $"OK: {value}",
    fail: errors => $"FAIL: {string.Join(", ", errors.Select(e => e.Code))}"
);
```

## Tap / TapError

Run side effects without changing the result.

```c#
result
    .Tap(v => logger.LogInformation("Success: {Value}", v))
    .TapError(errs => logger.LogWarning("Failed: {Count} errors", errs.Count));
```

## Ensure

Validate a successful value.  
If the predicate returns `false`, convert the result into a failure with the provided error.

```c#
var ensured = result.Ensure(
    predicate: x => x > 0,
    error: new Error(ErrorCodes.ValidationFormat, "Must be positive", ErrorType.Validation)
);
```

---

# LINQ query syntax

The package supports LINQ query syntax by providing:
- `Select` (alias for `Map`)
- `SelectMany` (alias for `Bind`)
- `Where` (predicate filter)

Example:

```c#
var res =
    from user in GetUser(userId)              // OperationResult<User>
    from token in IssueToken(user)            // OperationResult<Token>
    select new AuthDto(user.Id, token.Value);
```

## About `where`

C# query syntax `where` only accepts `Func<T, bool>` and does not allow passing a custom error.
Because of that, the default `Where(predicate)` typically returns a generic validation error
(e.g. `skopka.validation.predicate_failed`).

For real business rules, prefer `Ensure(predicate, error)` because it is explicit.

---

# Recommended usage

Use `OperationResult` / `OperationResult<T>` for:
- application services / use cases
- domain operations where failure is part of the contract
- validation pipelines and business rule checks

Use exceptions for:
- unexpected failures (IO, infrastructure, programming errors)
- truly exceptional situations you do not want to model as a result
