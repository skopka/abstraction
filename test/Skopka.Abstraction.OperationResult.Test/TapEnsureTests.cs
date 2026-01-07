namespace Skopka.Abstraction.OperationResult.Test;

public class TapEnsureTests
{
    private static readonly Error PredError =
        new(ErrorCodes.ValidationPredicateFailed, "Condition failed", ErrorType.Validation);
        
    [Fact]
    public void Tap_Action_OnSuccess_InvokesAndReturnsSame()
    {
        var res = OperationResultFactory.Success();

        var called = false;
        var same = res.Tap(() => called = true);

        Assert.True(called);
        Assert.Same(res, same);
    }
    
    [Fact]
    public void Tap_OnSuccess_InvokesAction()
    {
        var res = OperationResultFactory.Success(1);

        var called = false;
        var same = res.Tap(_ => called = true);

        Assert.True(called);
        Assert.Same(res, same);
    }

    [Fact]
    public void Tap_OnFail_DoesNotInvokeAction()
    {
        var res = OperationResultFactory.Fail<int>(new Error(ErrorCodes.Conflict, "Conflict", ErrorType.Conflict));

        var called = false;
        res.Tap(_ => called = true);

        Assert.False(called);
    }

    [Fact]
    public void TapError_OnFail_InvokesAction()
    {
        var res = OperationResultFactory.Fail<int>(new Error(ErrorCodes.Conflict, "Conflict", ErrorType.Conflict));

        var called = false;
        res.TapError(_ => called = true);

        Assert.True(called);
    }

    [Fact]
    public void TapError_OnSuccess_DoesNotInvokeAction()
    {
        var res = OperationResultFactory.Success(1);

        var called = false;
        res.TapError(_ => called = true);

        Assert.False(called);
    }
    
    [Fact]
    public void Ensure_OnSuccess_PredicateFalse_ReturnsFail()
    {
        var res = OperationResultFactory.Success();

        var ensured = res.Ensure(() => false, PredError);

        Assert.False(ensured.IsSuccess);
        var err = Assert.Single(ensured.Errors);
        Assert.Equal(ErrorCodes.ValidationPredicateFailed, err.Code);
    }

    [Fact]
    public void Ensure_OnFail_DoesNotInvokePredicate()
    {
        var res = OperationResultFactory.Fail(PredError);

        var called = false;
        var ensured = res.Ensure(() =>
        {
            called = true;
            return true;
        }, PredError);

        Assert.False(ensured.IsSuccess);
        Assert.False(called);
    }

    [Fact]
    public void Ensure_OnSuccess_PredicateTrue_ReturnsSameResult()
    {
        var res = OperationResultFactory.Success(10);

        var ensured = res.Ensure(x => x > 0, PredError);

        Assert.True(ensured.IsSuccess);
        Assert.Equal(10, ensured.Value);
    }

    [Fact]
    public void Ensure_OnSuccess_PredicateFalse_ReturnsFailWithProvidedError()
    {
        var res = OperationResultFactory.Success(0);

        var ensured = res.Ensure(x => x > 0, PredError);

        Assert.False(ensured.IsSuccess);
        Assert.Contains(ensured.Errors, e => e.Code == PredError.Code);
    }

    [Fact]
    public void Ensure_OnFail_DoesNotInvokePredicate_AndPropagatesErrors()
    {
        var err = new Error(ErrorCodes.Conflict, "Conflict", ErrorType.Conflict);
        var res = OperationResultFactory.Fail<int>(err);

        var called = false;
        var ensured = res.Ensure(_ =>
        {
            called = true;
            return true;
        }, PredError);

        Assert.False(ensured.IsSuccess);
        Assert.False(called);
        Assert.Equal(res.Errors, ensured.Errors);
    }
}