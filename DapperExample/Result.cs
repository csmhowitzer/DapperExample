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


