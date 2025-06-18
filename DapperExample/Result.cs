namespace DapperExample;

public readonly struct Result<TValue, TError>
{
    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    private readonly TValue? _value;
    private readonly TError? _error;

    public Result(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;
    }

    public Result(TError error)
    {
        IsError = true;
        _error = error;
        _value = default;
    }

    // implicit operator 
    // a way to define an implicit conversion operator between two types
    // allows for auto-type-conversion without the need to cast explicity
    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure)
    {
        // ! after arg will throw exception if arg is null
        return !IsError ? success(_value!) : failure(_error!);
    }
}

// Result Extensions
public static class FunctionalResult
{
    public static ProceduralResult<T2, TError> OGMap<T1, T2, TError>(this ProceduralResult<T1, TError> result, Func<T1, T2> map)
    {
        if (result.IsSuccess)
        {
            T2 mapped = map(result.Value);
            ProceduralResult<T2, TError> newResult = ProceduralResult<T2, TError>.Success(mapped);
            return newResult;
        }
        return ProceduralResult<T2, TError>.Failure(result.Error);
    }

    // transform the value into something else, assumes the map is on the success path
    public static ProceduralResult<T2, TError> Map<T1, T2, TError>(this ProceduralResult<T1, TError> result, Func<T1, T2> map) =>
        result.IsSuccess
            ? ProceduralResult<T2, TError>.Success(map(result.Value))
            : ProceduralResult<T2, TError>.Failure(result.Error);

    // transform the error into something else, assumes the map is on the failure path
    public static ProceduralResult<T, TNewError> MapError<T, TError, TNewError>(this ProceduralResult<T, TError> result, Func<TError, TNewError> map) =>
        result.IsSuccess
            ? ProceduralResult<T, TNewError>.Success(result.Value)
            : ProceduralResult<T, TNewError>.Failure(map(result.Error));

    // transform the result into something else and return it
    public static TResult Match<T, TError, TResult>(this ProceduralResult<T, TError> result, Func<T, TResult> mapValue, Func<TError, TResult> mapError) =>
        result.IsSuccess
            ? mapValue(result.Value)
            : mapError(result.Error);

    // bind the result to another result, produces a new result rather than a value. Similar to Map()
    public static ProceduralResult<T2, TError> Bind<T1, T2, TError>(this ProceduralResult<T1, TError> result, Func<T1, ProceduralResult<T2, TError>> bind) =>
        result.IsSuccess
            ? bind(result.Value)
            : ProceduralResult<T2, TError>.Failure(result.Error);
}

public class ProceduralResult<TValue, TError>
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

    private ProceduralResult(bool isSuccess, TValue? value, TError? error)
    {
        IsSuccess = isSuccess;
        _value = value;
        _error = error;
    }

    public static ProceduralResult<TValue, TError> Success(TValue value) => new(true, value, default);
    public static ProceduralResult<TValue, TError> Failure(TError error) => new(false, default, error);

}
