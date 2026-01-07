namespace Skopka.Abstraction.OperationResult;

public static class OperationResultLinqExtensions
{
    extension<TSource>(OperationResult<TSource> result)
    {
        public OperationResult<TResult> Select<TResult>(Func<TSource, TResult> selector)
            => result.Map(selector);
        
        public OperationResult<TResult> SelectMany<TResult>(Func<TSource, OperationResult<TResult>> binder)
            => result.Bind(binder);
        
        public OperationResult<TResult> SelectMany<TIntermediate, TResult>(
            Func<TSource, OperationResult<TIntermediate>> binder,
            Func<TSource, TIntermediate, TResult> projector)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(binder);
            ArgumentNullException.ThrowIfNull(projector);

            return result.Bind(x => binder(x).Map(y => projector(x, y)));
        }

        public OperationResult<TSource> Where(Func<TSource, bool> predicate, Error error)
            => result.Ensure(predicate, error);
        
        public OperationResult<TSource> Where(Func<TSource, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(result);
            ArgumentNullException.ThrowIfNull(predicate);

            if (!result.IsSuccess)
                return result;

            return predicate(result.Value)
                ? result
                : OperationResultFactory.Fail<TSource>(
                        ErrorCodes.ValidationPredicateFailed,
                        "Condition failed",
                        ErrorType.Validation, 
                        new { Rule = "where" });
        }
    }
}