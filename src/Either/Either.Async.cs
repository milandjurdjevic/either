using System;
using System.Threading.Tasks;

namespace Either;

public static partial class Either
{
    public static async Task<Either<TOk2, TError>> SelectAsync<TOk1, TOk2, TError>(
        this Task<Either<TOk1, TError>> self,
        Func<TOk1, TOk2> map
    ) => (await self).Select(map);

    public static Task<Either<TOk2, TError>> SelectAsync<TOk1, TOk2, TError>(
        this Either<TOk1, TError> self,
        Func<TOk1, Task<TOk2>> map
    ) => self.Match<Task<Either<TOk2, TError>>>(
        async ok => await map(ok),
        async err => await Task.FromResult(err)
    );

    public static async Task<Either<TOk2, TError>> SelectAsync<TOk1, TOk2, TError>(
        this Task<Either<TOk1, TError>> self,
        Func<TOk1, Task<TOk2>> map
    ) => await (await self).SelectAsync(map);
}