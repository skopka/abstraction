namespace Skopka.Abstraction.OperationResult;

public sealed record Error(
    string Code,
    string Message,
    ErrorType Type = ErrorType.Failure,
    object? Details = null);