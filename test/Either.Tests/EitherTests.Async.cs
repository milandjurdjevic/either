using JetBrains.Annotations;

namespace Either.Tests;

[TestSubject(typeof(Either))]
[TestSubject(typeof(Either<,>))]
public partial class EitherTests
{
    [Fact]
    public async Task SelectAsync1_Ok_Ok()
    {
        const string expected = "success";
        var select = await EitherAsync.Ok().SelectAsync(_ => expected);
        var match = select.Match<string>(ok => ok, err => err);
        Assert.Equal(expected, match);
    }

    [Fact]
    public async Task SelectAsync2_Ok_Ok()
    {
        var select = await EitherSync.Ok().SelectAsync(async _ =>
        {
            await EitherAsync.Delay;
            return "success";
        });

        Assert.Equal("success", select.Match<string>(ok => ok, err => err));
    }

    [Fact]
    public async Task SelectAsync3_Ok_Ok()
    {
        var select = await EitherAsync.Ok().SelectAsync(async _ =>
        {
            await EitherAsync.Delay;
            return "success";
        });

        Assert.Equal("success", select.Match<string>(ok => ok, err => err));
    }

    [Fact]
    public async Task SelectAsync1_Error_Error()
    {
        var select = await EitherAsync.Error().SelectAsync(_ => "success");
        var match = select.Match<string>(ok => ok, err => err);
        Assert.Equal("error", match);
    }

    [Fact]
    public async Task SelectAsync2_Error_Error()
    {
        var select = await EitherSync.Error().SelectAsync(async _ =>
        {
            await EitherAsync.Delay;
            return "success";
        });

        Assert.Equal("error", select.Match<string>(ok => ok, err => err));
    }

    [Fact]
    public async Task SelectAsync3_Error_Error()
    {
        var select = await EitherAsync.Error().SelectAsync(async _ =>
        {
            await EitherAsync.Delay;
            return "success";
        });

        Assert.Equal("error", select.Match<string>(ok => ok, err => err));
    }
}

file static class EitherSync
{
    public static Either<Unit, string> Error() => Either.Error<Unit, string>("error");
    public static Either<Unit, string> Ok() => Either.Ok<Unit, string>(Unit.Value);
}

file static class EitherAsync
{
    public static Task Delay => Task.Delay(TimeSpan.FromSeconds(1));

    public static async Task<Either<Unit, string>> Ok()
    {
        await Delay;
        return Unit.Value;
    }


    public static async Task<Either<Unit, string>> Error()
    {
        await Delay;
        return "error";
    }
}