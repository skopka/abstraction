namespace Skopka.Abstraction.OperationResult;

public class OperationResult
{
    internal OperationResult()
    {
        IsSuccess = true; 
        Errors = [];
    }

    internal OperationResult(IReadOnlyCollection<Error> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }
    
    public bool IsSuccess { get; }
    public IReadOnlyCollection<Error> Errors { get; }
}

public sealed class OperationResult<T> : OperationResult
{
    internal OperationResult(T value)
    {
        Value = value;
    }

    internal OperationResult(IReadOnlyCollection<Error> errors) : base(errors)
    {
        Value = default!;
    }

    public T Value => IsSuccess ? field : throw new InvalidOperationException("Value is not set");
}