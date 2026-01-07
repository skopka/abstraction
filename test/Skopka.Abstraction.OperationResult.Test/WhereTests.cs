namespace Skopka.Abstraction.OperationResult.Test;

public class WhereTests
{
    [Fact]
    public void Where_OnSuccess_PredicateTrue_ReturnsSuccess()
    {
        var res = OperationResultFactory.Success(10);

        var filtered = res.Where(x => x > 0);

        Assert.True(filtered.IsSuccess);
        Assert.Equal(10, filtered.Value);
    }

    [Fact]
    public void Where_OnSuccess_PredicateFalse_ReturnsFailWithDefaultError()
    {
        var res = OperationResultFactory.Success(0);

        var filtered = res.Where(x => x > 0);

        Assert.False(filtered.IsSuccess);

        var err = Assert.Single(filtered.Errors);
        Assert.Equal(ErrorCodes.ValidationPredicateFailed, err.Code);
        Assert.Equal(ErrorType.Validation, err.Type);
    }

    [Fact]
    public void Where_OnFail_DoesNotInvokePredicate_AndPropagatesErrors()
    {
        var res = OperationResultFactory.Fail<int>(new Error(ErrorCodes.Conflict, "Conflict", ErrorType.Conflict));

        var called = false;
        var filtered = res.Where(_ =>
        {
            called = true;
            return true;
        });

        Assert.False(filtered.IsSuccess);
        Assert.False(called);
        Assert.Equal(res.Errors, filtered.Errors);
    }

    [Fact]
    public void QuerySyntax_WithWhere_Works()
    {
        var res =
            from x in OperationResultFactory.Success(1)
            where x > 5
            select x;

        Assert.False(res.IsSuccess);
        var err = Assert.Single(res.Errors);
        Assert.Equal(ErrorCodes.ValidationPredicateFailed, err.Code);
    }
}