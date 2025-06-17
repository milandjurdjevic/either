using JetBrains.Annotations;

namespace Either.Tests;

using static Either;

[TestSubject(typeof(Either))]
[TestSubject(typeof(Either<,>))]
public partial class EitherTests
{
    [Fact]
    public void Match_Error_Error()
    {
        var match = Error<Unit, Unit>(Unit.Value)
            .Match(_ => true, _ => false);

        Assert.False(match);
    }

    [Fact]
    public void Match_Ok_Ok()
    {
        var match = Ok<Unit, Unit>(Unit.Value)
            .Match(_ => true, _ => false);

        Assert.True(match);
    }

    [Fact]
    public void Select_Ok_Ok()
    {
        var match = Ok<bool, bool>(false)
            .Select(_ => true)
            .Match(ok => ok, err => err);

        Assert.True(match);
    }

    [Fact]
    public void Select_Error_Error()
    {
        var match = Error<bool, bool>(false)
            .Select(_ => false)
            .Match(ok => ok, err => err);

        Assert.False(match);
    }

    [Fact]
    public void SelectFlat_Ok_Ok()
    {
        var match = Ok<int, int>(10)
            .SelectFlat(ok => Ok<int, int>(ok * 2))
            .Match(ok => ok, _ => 0);

        Assert.Equal(20, match);
    }

    [Fact]
    public void SelectFlat_OkToError_Error()
    {
        var match = Ok<int, string>(1)
            .SelectFlat(_ => Error<int, string>("error"))
            .Match(ok => ok, _ => -1);

        Assert.Equal(-1, match);
    }

    [Fact]
    public void SelectFlat_Error_Error()
    {
        var match = Error<int, string>("error")
            .SelectFlat(x => Ok<int, string>(x * 2))
            .Match(ok => ok, _ => -1);

        Assert.Equal(-1, match);
    }
}