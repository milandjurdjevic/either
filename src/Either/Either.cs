using System;

namespace Either;

public abstract class Either<TOk, TError>
{
    private Either() { }

    public abstract TResult Match<TResult>(Func<TOk, TResult> ok, Func<TError, TResult> error);

    public static implicit operator Either<TOk, TError>(TOk ok) => new Ok(ok);

    public static implicit operator Either<TOk, TError>(TError error) => new Error(error);

    private class Error(TError value) : Either<TOk, TError>
    {
        public override TResult Match<TResult>(Func<TOk, TResult> ok, Func<TError, TResult> error) => error(value);
    }

    private class Ok(TOk value) : Either<TOk, TError>
    {
        public override TResult Match<TResult>(Func<TOk, TResult> ok, Func<TError, TResult> error) => ok(value);
    }
}

public static class Either
{
    public static Either<TOk, TError> Ok<TOk, TError>(TOk ok) => ok;

    public static Either<TOk, TError> Error<TOk, TError>(TError error) => error;

    public static Either<TOk2, TError> Select<TOk1, TOk2, TError>(
        this Either<TOk1, TError> either,
        Func<TOk1, TOk2> map
    ) => either.Match(ok => Ok<TOk2, TError>(map(ok)), Error<TOk2, TError>);

    public static Either<TOk2, TError> SelectFlat<TOk1, TOk2, TError>(
        this Either<TOk1, TError> either,
        Func<TOk1, Either<TOk2, TError>> map
    ) => either.Match(map, Error<TOk2, TError>);

    public static Either<TOk3, TError> SelectMany<TOk1, TOk2, TOk3, TError>(
        this Either<TOk1, TError> either,
        Func<TOk1, Either<TOk2, TError>> flat,
        Func<TOk1, TOk2, TOk3> select
    ) => either.SelectFlat(ok1 => flat(ok1).Select(ok2 => select(ok1, ok2)));
}