namespace Skopka.Abstraction.OperationResult;

public static class OperationResultExtensions
{
    extension<TSource>(OperationResult<TSource> result)
    {
        public OperationResult<TResult> Map<TResult>(Func<TSource, TResult> map)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(map);

            return result.IsSuccess
                ? OperationResultFactory.Success(map(result.Value))
                : OperationResultFactory.Fail<TResult>(result.Errors);
        }

        public OperationResult<TResult> Bind<TResult>(Func<TSource, OperationResult<TResult>> bind)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(bind);

            return result.IsSuccess
                ? bind(result.Value)
                : OperationResultFactory.Fail<TResult>(result.Errors);
        }

        public OperationResult Bind(Func<TSource, OperationResult> bind)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(bind);

            return result.IsSuccess
                ? bind(result.Value)
                : OperationResultFactory.Fail(result.Errors);
        }

        public TResult Match<TResult>(Func<TSource, TResult> ok, Func<IReadOnlyCollection<Error>, TResult> fail)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(ok);
            ArgumentNullException.ThrowIfNull(fail);

            return result.IsSuccess
                ? ok(result.Value)
                : fail(result.Errors);
        }

        public void Match(Action<TSource> ok, Action<IReadOnlyCollection<Error>> fail)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(ok);
            ArgumentNullException.ThrowIfNull(fail);

            if (result.IsSuccess) ok(result.Value);
            else fail(result.Errors);
        }

        public OperationResult<TSource> Tap(Action<TSource> onSuccess)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(onSuccess);

            if (result.IsSuccess) onSuccess(result.Value);
            return result;
        }

        public OperationResult<TSource> TapError(Action<IReadOnlyCollection<Error>> onFailure)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(onFailure);

            if (!result.IsSuccess) onFailure(result.Errors);
            return result;
        }

        public OperationResult<TSource> Ensure(Func<TSource, bool> predicate, Error error)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(predicate);

            if (!result.IsSuccess)
                return result;

            return predicate(result.Value)
                ? result
                : OperationResultFactory.Fail<TSource>(error);
        }

        public OperationResult<TSource> Ensure(
            Func<TSource, bool> predicate,
            string errorCode,
            string errorMessage,
            ErrorType errorType,
            object? errorDetails = null)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(predicate);

            return result.Ensure(predicate, new Error(errorCode, errorMessage, errorType, errorDetails));
        }
    }

    extension(OperationResult result)
    {
        public OperationResult<TResult> Map<TResult>(Func<TResult> map)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(map);

            return result.IsSuccess
                ? OperationResultFactory.Success(map())
                : OperationResultFactory.Fail<TResult>(result.Errors);
        }

        public OperationResult<TResult> Map<TResult>(TResult value)
        {
            ArgumentNullException.ThrowIfNull(result);

            return result.IsSuccess
                ? OperationResultFactory.Success(value)
                : OperationResultFactory.Fail<TResult>(result.Errors);
        }
        
        public OperationResult<TResult> Bind<TResult>(Func<OperationResult<TResult>> bind)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(bind);

            return result.IsSuccess
                ? bind()
                : OperationResultFactory.Fail<TResult>(result.Errors);
        }

        public OperationResult Bind(Func<OperationResult> bind)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(bind);

            return result.IsSuccess
                ? bind()
                : OperationResultFactory.Fail(result.Errors);
        }

        public OperationResult Tap(Action onSuccess)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(onSuccess);

            if (result.IsSuccess) onSuccess();
            return result;
        }

        public OperationResult Ensure(Func<bool> predicate, Error error)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(predicate);

            if (!result.IsSuccess)
                return result;

            return predicate()
                ? result
                : OperationResultFactory.Fail(error);
        }

        public OperationResult Ensure(
            Func<bool> predicate,
            string errorCode,
            string errorMessage,
            ErrorType errorType,
            object? errorDetails = null)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(predicate);

            return result.Ensure(predicate, new Error(errorCode, errorMessage, errorType, errorDetails));
        }

        public TResult Match<TResult>(Func<TResult> ok, Func<IReadOnlyCollection<Error>, TResult> fail)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(ok);
            ArgumentNullException.ThrowIfNull(fail);

            return result.IsSuccess
                ? ok()
                : fail(result.Errors);
        }

        public void Match(Action ok, Action<IReadOnlyCollection<Error>> fail)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(ok);
            ArgumentNullException.ThrowIfNull(fail);

            if (result.IsSuccess) ok();
            else fail(result.Errors);
        }
    }
}
