namespace DapperExample;

// public readonly struct Result<TValue, TError>
// {
//     public bool IsError { get; }
//     public bool IsSuccess => !IsError;
//
//     private readonly TValue? _value;
//     private readonly TError? _error;
//
//     public Result(TValue value)
//     {
//         IsError = false;
//         _value = value;
//         _error = default;
//     }
//
//     public Result(TError error)
//     {
//         IsError = true;
//         _error = error;
//         _value = default;
//     }
//
//     // implicit operator 
//     // a way to define an implicit conversion operator between two types
//     // allows for auto-type-conversion without the need to cast explicity
//     public static implicit operator Result<TValue, TError>(TValue value) => new(value);
//     public static implicit operator Result<TValue, TError>(TError error) => new(error);
//
//     public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure)
//     {
//         // ! after arg will throw exception if arg is null
//         return !IsError ? success(_value!) : failure(_error!);
//     }
// }

public class Result<TValue, TError>
{
    private TValue? _value;
    private TError? _error;

    public bool IsSuccess { get; }

    public TValue Value
    {
        get => IsSuccess ? _value! : throw new InvalidOperationException("Result not successful");
        private set => _value = value;
    }

    public TError Error
    {
        get => !IsSuccess ? _error! : throw new InvalidOperationException("Result is successful");
        private set => _error = value;
    }

    private Result(bool isSuccess, TValue? value, TError? error)
    {
        IsSuccess = isSuccess;
        _value = value;
        _error = error;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(true, value, default);
    public static implicit operator Result<TValue, TError>(TError error) => new(false, default, error);

}

// Result Extensions
public static class FunctionalResult
{
    // transform the value into something else, assumes the map is on the success path
    public static Result<T2, TError> Map<T1, T2, TError>(this Result<T1, TError> result, Func<T1, T2> map) =>
        result.IsSuccess
            ? map(result.Value)
            : result.Error;

    // transform the error into something else, assumes the map is on the failure path
    public static Result<T, TNewError> MapError<T, TError, TNewError>(this Result<T, TError> result, Func<TError, TNewError> map) =>
        result.IsSuccess
            ? result.Value
            : map(result.Error);

    // transform the result into something else and return it
    public static TResult Match<T, TError, TResult>(this Result<T, TError> result, Func<T, TResult> mapValue, Func<TError, TResult> mapError) =>
        result.IsSuccess
            ? mapValue(result.Value)
            : mapError(result.Error);

    // bind the result to another result, produces a new result rather than a value. Similar to Map()
    public static Result<T2, TError> Bind<T1, T2, TError>(this Result<T1, TError> result, Func<T1, Result<T2, TError>> bind) =>
        result.IsSuccess
            ? bind(result.Value)
            : result.Error;
}


public static class AsyncFunctionalResult
{
    public static async Task<Result<T2, TError>> MapAsync<T1, T2, TError>(this Task<Result<T1, TError>> result, Func<T1, T2> map) =>
        (await result).Map(map);

    public static async Task<Result<T2, TError>> MapAsync<T1, T2, TError>(this Result<T1, TError> result, Func<T1, Task<T2>> mapAsync) =>
        result.IsSuccess
            ? await mapAsync(result.Value)
            : result.Error;

    public static async Task<Result<T2, TError>> MapAsync<T1, T2, TError>(this Task<Result<T1, TError>> result, Func<T1, Task<T2>> mapAsync) =>
        await (await result).MapAsync(mapAsync);

    public static async Task<Result<T2, TError>> BindAsync<T1, T2, TError>(this Task<Result<T1, TError>> result, Func<T1, Result<T2, TError>> bind) =>
        (await result).Bind(bind);

    public static async Task<Result<T2, TError>> BindAsync<T1, T2, TError>(this Result<T1, TError> result, Func<T1, Task<Result<T2, TError>>> bindAsync) =>
        result.IsSuccess
            ? await bindAsync(result.Value)
            : result.Error;

    public static async Task<Result<T2, TError>> BindAsync<T1, T2, TError>(this Task<Result<T1, TError>> result, Func<T1, Task<Result<T2, TError>>> bindAsync) =>
        await (await result).BindAsync(bindAsync);

    public static async Task<Result<T, TNewError>> MapErrorAsync<T, TError, TNewError>(this Task<Result<T, TError>> result, Func<TError, TNewError> map) =>
        (await result).MapError(map);

    public static async Task<Result<T, TNewError>> MapErrorAsync<T, TError, TNewError>(this Result<T, TError> result, Func<TError, Task<TNewError>> mapAsync) =>
        result.IsSuccess
            ? result.Value
            : await mapAsync(result.Error);

    public static async Task<Result<T, TNewError>> MapErrorAsync<T, TError, TNewError>(this Task<Result<T, TError>> result, Func<TError, Task<TNewError>> mapAsync) =>
        await (await result).MapErrorAsync(mapAsync);

    public static async Task<TResult> MatchAsync<T, TError, TResult>(this Task<Result<T, TError>> result, Func<T, TResult> onSuccess, Func<TError, TResult> onFailure) =>
        (await result).Match(onSuccess, onFailure);

    public static async Task<TResult> MatchAsync<T, TError, TResult>(this Result<T, TError> result, Func<T, Task<TResult>> onSuccessAsync, Func<TError, TResult> onFailure) =>
        result.IsSuccess
            ? await onSuccessAsync(result.Value)
            : onFailure(result.Error);

    public static async Task<TResult> MatchAsync<T, TError, TResult>(this Result<T, TError> result, Func<T, TResult> onSuccess, Func<TError, Task<TResult>> onFailureAsync) =>
        result.IsSuccess
            ? onSuccess(result.Value)
            : await onFailureAsync(result.Error);

    public static async Task<TResult> MatchAsync<T, TError, TResult>(this Task<Result<T, TError>> result, Func<T, Task<TResult>> onSuccessAsync, Func<TError, TResult> onFailure) =>
        await (await result).MatchAsync(onSuccessAsync, onFailure);

    public static async Task<TResult> MatchAsync<T, TError, TResult>(this Task<Result<T, TError>> result, Func<T, TResult> onSuccess, Func<TError, Task<TResult>> onFailureAsync) =>
        await (await result).MatchAsync(onSuccess, onFailureAsync);

    public static async Task<TResult> MatchAsync<T, TError, TResult>(this Result<T, TError> result, Func<T, Task<TResult>> onSuccessAsync, Func<TError, Task<TResult>> onFailureAsync) =>
        result.IsSuccess
            ? await onSuccessAsync(result.Value)
            : await onFailureAsync(result.Error);

    public static async Task<TResult> MatchAsync<T, TError, TResult>(this Task<Result<T, TError>> result, Func<T, Task<TResult>> onSuccessAsync, Func<TError, Task<TResult>> onFailureAsync) =>
        await (await result).MatchAsync(onSuccessAsync, onFailureAsync);
}
