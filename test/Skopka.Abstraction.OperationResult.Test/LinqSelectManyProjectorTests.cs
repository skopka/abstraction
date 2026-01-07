namespace Skopka.Abstraction.OperationResult.Test;

public class LinqSelectManyProjectorTests
{
    [Fact]
    public void Select_IsSameAsMap()
    {
        var res = OperationResultFactory.Success(2);

        var selected = res.Select(x => x * 10);

        Assert.True(selected.IsSuccess);
        Assert.Equal(20, selected.Value);
    }

    [Fact]
    public void SelectMany_Binder_IsSameAsBind()
    {
        var res = OperationResultFactory.Success(3);

        var bound = res.SelectMany(x => OperationResultFactory.Success(x + 7));

        Assert.True(bound.IsSuccess);
        Assert.Equal(10, bound.Value);
    }

    [Fact]
    public void SelectMany_WithProjector_ComposesTwoResults()
    {
        var res = OperationResultFactory.Success(5);

        var composed = res.SelectMany(
            binder: x => OperationResultFactory.Success(x * 2),
            projector: (x, y) => x + y);

        Assert.True(composed.IsSuccess);
        Assert.Equal(15, composed.Value);
    }

    [Fact]
    public void QuerySyntax_TwoFrom_SelectProjector_Works()
    {
        var res =
            from x in OperationResultFactory.Success(2)
            from y in OperationResultFactory.Success(10)
            select x + y;

        Assert.True(res.IsSuccess);
        Assert.Equal(12, res.Value);
    }

    [Fact]
    public void QuerySyntax_TwoFrom_SecondFails_PropagatesErrors()
    {
        var fail = OperationResultFactory.Fail<int>(new Error(ErrorCodes.Conflict, "Conflict", ErrorType.Conflict));

        var res =
            from x in OperationResultFactory.Success(2)
            from y in fail
            select x + y;

        Assert.False(res.IsSuccess);
        var err = Assert.Single(res.Errors);
        Assert.Equal(ErrorCodes.Conflict, err.Code);
    }

    [Fact]
    public void QuerySyntax_Where_FiltersToFailWithDefaultPredicateError()
    {
        var res =
            from x in OperationResultFactory.Success(1)
            where x > 10
            select x;

        Assert.False(res.IsSuccess);

        var err = Assert.Single(res.Errors);
        Assert.Equal(ErrorCodes.ValidationPredicateFailed, err.Code);
        Assert.Equal(ErrorType.Validation, err.Type);
    }

    [Fact]
    public void QuerySyntax_Where_DoesNotRunIfUpstreamFailed()
    {
        var upstream = OperationResultFactory.Fail<int>(new Error(ErrorCodes.NotFound, "Not found", ErrorType.NotFound));

        var called = false;

        var res =
            from x in upstream
            where x > 0 && (called = true)
            select x;

        Assert.False(res.IsSuccess);
        Assert.False(called);

        var err = Assert.Single(res.Errors);
        Assert.Equal(ErrorCodes.NotFound, err.Code);
    }
}
