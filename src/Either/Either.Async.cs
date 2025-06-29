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

    public static async Task<Either<TOk2, TError>> SelectFlatAsync<TOk1, TOk2, TError>(
        this Task<Either<TOk1, TError>> self,
        Func<TOk1, Either<TOk2, TError>> map
    ) => (await self).SelectFlat(map);

    public static Task<Either<TOk2, TError>> SelectFlatAsync<TOk1, TOk2, TError>(
        this Either<TOk1, TError> self,
        Func<TOk1, Task<Either<TOk2, TError>>> map
    ) => self.Match<Task<Either<TOk2, TError>>>(async ok => await map(ok), async err => await Task.FromResult(err));

    public static async Task<Either<TOk2, TError>> SelectFlatAsync<TOk1, TOk2, TError>(
        this Task<Either<TOk1, TError>> self,
        Func<TOk1, Task<Either<TOk2, TError>>> map
    ) => await (await self).SelectFlatAsync(map);
    
    public static async Task<Either<TOk, TError2>> CatchAsync<TOk, TError1, TError2>(
        this Task<Either<TOk, TError1>> self,
        Func<TError1, TError2> map
    ) => (await self).Catch(map);
}