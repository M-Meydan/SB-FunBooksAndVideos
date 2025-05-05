namespace SharedContracts.Result;

/// <summary>
/// Result class for operations that do not return a value.
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Ok()
    {
        return new Result(true, string.Empty);
    }

    public static Result Fail(string error)
    {
        return new Result(false, error);
    }
}

public class Result<T> : Result
{
    private readonly T _value;
    public T Value
    {
        get
        {
            if (!IsSuccess)
                throw new InvalidOperationException("Cannot access the value of a failed result.");
            return _value;
        }
    }

    private Result(bool isSuccess, string error, T value) : base(isSuccess, error)
    {
        _value = value;
    }

    public static Result<T> Ok(T value)
    {
        return new Result<T>(true, string.Empty, value);
    }

    public static new Result<T> Fail(string error)
    {
        return new Result<T>(false, error, default);
    }
}

