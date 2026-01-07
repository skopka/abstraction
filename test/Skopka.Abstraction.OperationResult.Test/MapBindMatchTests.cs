namespace Skopka.Abstraction.OperationResult.Test;

public class MapBindMatchTests
{
    private static readonly IReadOnlyCollection<Error> Errors = new List<Error> { new(ErrorCodes.Conflict, "Conflict", ErrorType.Conflict) };

    [Fact]
    public void Map_Action_OnFail_DoesNotInvoke()
    {
        var res = OperationResultFactory.Fail(Errors);

        var called = false;
        res.Map(() => called = true);

        Assert.False(called);
    }

    [Fact]
    public void Map_Func_OnSuccess_ReturnsSuccessValue()
    {
        var res = OperationResultFactory.Success();

        var mapped = res.Map(() => 123);

        Assert.True(mapped.IsSuccess);
        Assert.Equal(123, mapped.Value);
    }

    [Fact]
    public void Map_Value_OnFail_PropagatesErrors()
    {
        var res = OperationResultFactory.Fail(Errors);

        var mapped = res.Map(999);

        Assert.False(mapped.IsSuccess);
        var err = Assert.Single(mapped.Errors);
        Assert.Equal(ErrorCodes.Conflict, err.Code);
    }

    [Fact]
    public void Map_OnSuccess_MapsValue()
    {
        var res = OperationResultFactory.Success(10);

        var mapped = res.Map(x => x.ToString());

        Assert.True(mapped.IsSuccess);
        Assert.Equal("10", mapped.Value);
    }

    [Fact]
    public void Map_OnFail_PropagatesErrors_AndDoesNotInvokeDelegate()
    {
        var res = OperationResultFactory.Fail<int>(Errors);

        var called = false;
        var mapped = res.Map(x =>
        {
            called = true;
            return x.ToString();
        });

        Assert.False(mapped.IsSuccess);
        Assert.False(called);
        Assert.Equal(res.Errors, mapped.Errors);
    }
    
    [Fact]
    public void Bind_NonGeneric_OnSuccess_ExecutesBinder()
    {
        var res = OperationResultFactory.Success();

        var called = false;
        var bound = res.Bind(() =>
        {
            called = true;
            return OperationResultFactory.Success();
        });

        Assert.True(called);
        Assert.True(bound.IsSuccess);
    }

    [Fact]
    public void Bind_NonGeneric_OnFail_DoesNotInvokeBinder()
    {
        var res = OperationResultFactory.Fail(Errors);

        var called = false;
        var bound = res.Bind(() =>
        {
            called = true;
            return OperationResultFactory.Success();
        });

        Assert.False(called);
        Assert.False(bound.IsSuccess);
    }

    [Fact]
    public void Bind_ToGeneric_OnSuccess_ReturnsValue()
    {
        // Требует extension: OperationResult.Bind<TResult>(Func<OperationResult<TResult>>)
        var res = OperationResultFactory.Success();

        var bound = res.Bind(() => OperationResultFactory.Success("ok"));

        Assert.True(bound.IsSuccess);
        Assert.Equal("ok", bound.Value);
    }


    [Fact]
    public void Bind_OnSuccess_ReturnsBinderResult()
    {
        var res = OperationResultFactory.Success(5);

        var bound = res.Bind(x => OperationResultFactory.Success(x * 2));

        Assert.True(bound.IsSuccess);
        Assert.Equal(10, bound.Value);
    }

    [Fact]
    public void Bind_OnFail_PropagatesErrors_AndDoesNotInvokeBinder()
    {
        var res = OperationResultFactory.Fail<int>(Errors);

        var called = false;
        var bound = res.Bind(x =>
        {
            called = true;
            return OperationResultFactory.Success(x * 2);
        });

        Assert.False(bound.IsSuccess);
        Assert.False(called);
        Assert.Equal(res.Errors, bound.Errors);
    }
    
    [Fact]
    public void NonGeneric_Match_OnSuccess_CallsOk()
    {
        var res = OperationResultFactory.Success();

        var v = res.Match(
            ok: () => 1,
            fail: _ => -1);

        Assert.Equal(1, v);
    }

    [Fact]
    public void NonGeneric_Match_OnFail_CallsFail()
    {
        var res = OperationResultFactory.Fail(Errors);

        var v = res.Match(
            ok: () => 1,
            fail: errs => errs.Count);

        Assert.Equal(1, v);
    }

    [Fact]
    public void Match_OnSuccess_CallsOk()
    {
        var res = OperationResultFactory.Success(7);

        var v = res.Match(
            ok: x => x + 1,
            fail: _ => -1);

        Assert.Equal(8, v);
    }

    [Fact]
    public void Match_OnFail_CallsFail()
    {
        var res = OperationResultFactory.Fail<int>(Errors);

        var v = res.Match(
            ok: _ => 123,
            fail: errs => errs.Count);

        Assert.Equal(1, v);
    }
}