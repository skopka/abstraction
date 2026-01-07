using System.Collections.ObjectModel;

namespace Skopka.Abstraction.OperationResult;

public class FailedOperationResultBuilder
{
    private List<Error> _errors;

    public FailedOperationResultBuilder()
    {
        _errors = [];
    }
    
    public FailedOperationResultBuilder(int errorCount)
    {
        _errors = new List<Error>(errorCount);
    }
    
    public void AddError(Error error) => _errors.Add(error);
    public void AddError(string errorCode, string errorMessage, ErrorType errorType, object? errorDetails = null) => _errors.Add(new Error(errorCode, errorMessage, errorType, errorDetails));
        
    public void AddErrors(params IEnumerable<Error> errors) => _errors.AddRange(errors);
    public void Clear() => _errors.Clear();
    
    public OperationResult Build() => new(GetErrors());
    public OperationResult<T> Build<T>() => new(GetErrors());

    private ReadOnlyCollection<Error> GetErrors() => _errors.Count > 0 ? _errors.AsReadOnly() : new List<Error> { new (ErrorCodes.Unknown, "Unknown error") }.AsReadOnly();
}