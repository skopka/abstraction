namespace Skopka.Abstraction.OperationResult;

public static class OperationResultFactory
{
    private static readonly OperationResult SuccessOperationResult = new();
    private static readonly Error Unknown = new (ErrorCodes.Unknown, "Unknown error");
    
    public static OperationResult Success() => SuccessOperationResult;    
    public static OperationResult<T> Success<T>(T value) => new(value);
    public static OperationResult Fail(params IEnumerable<Error> errors) => new(errors.ToArray().AsReadOnly());
    public static OperationResult<T> Fail<T>(params IEnumerable<Error> errors) => new(errors.ToArray().AsReadOnly());
    public static OperationResult Fail(string errorCode, string errorMessage, ErrorType errorType, object? errorDetails = null)
        => Fail(new Error(errorCode, errorMessage, errorType, errorDetails));
    public static OperationResult<T> Fail<T>(string errorCode, string errorMessage, ErrorType errorType, object? errorDetails = null)
        => Fail<T>(new Error(errorCode, errorMessage, errorType, errorDetails));

    public static OperationResult Combine(params OperationResult[] results)
    {
        ArgumentNullException.ThrowIfNull(results);

        if (results.Length == 0)
            return Success();

        List<Error>? errors = [];
        var isSuccess = true;

        foreach (var r in results)
        {
            if (r.IsSuccess) continue;
            
            isSuccess = false;
            if (r.Errors is { Count: > 0 })
                errors.AddRange(r.Errors);
        }

        if (isSuccess)
            return Success();

        if (errors.Count == 0)
            return Fail(Unknown);

        return Fail(errors);
    }
    
    public static OperationResult Combine<T>(params OperationResult<T>[] results)
    {
        ArgumentNullException.ThrowIfNull(results);
        return Combine(results.Cast<OperationResult>().ToArray());
    }
}