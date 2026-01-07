namespace Skopka.Abstraction.OperationResult.Test;

public class OperationResultValueTests
{
    [Fact]
    public void Success_Value_ReturnsValue()
    {
        var id = Guid.NewGuid();
        var res = OperationResultFactory.Success(id);

        Assert.True(res.IsSuccess);
        Assert.Equal(id, res.Value);
    }

    [Fact]
    public void Fail_Value_Throws()
    {
        var errors = new List<Error>
        {
            new(ErrorCodes.Conflict, "Conflict", ErrorType.Conflict),
        };

        var res = OperationResultFactory.Fail<Guid>(errors);

        Assert.False(res.IsSuccess);
        Assert.Throws<InvalidOperationException>(() => res.Value);
    }

    [Fact]
    public void Fail_Errors_AreExposed()
    {
        var errors = new List<Error>
        {
            new(ErrorCodes.NotFound, "Not found", ErrorType.NotFound),
            new(ErrorCodes.Conflict, "Conflict", ErrorType.Conflict),
        };

        var res = OperationResultFactory.Fail<Guid>(errors);

        Assert.False(res.IsSuccess);
        Assert.Equal(2, res.Errors.Count);
    }
}