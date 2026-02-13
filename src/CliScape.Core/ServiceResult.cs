namespace CliScape.Core;

/// <summary>
///     A generic result type for service operations that may succeed or fail.
///     Replaces ad-hoc tuple returns from service validation methods.
/// </summary>
/// <typeparam name="T">The type of the value on success.</typeparam>
public readonly record struct ServiceResult<T>
{
    /// <summary>
    ///     Whether the operation succeeded.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    ///     A human-readable message describing the outcome.
    /// </summary>
    public string Message { get; }

    /// <summary>
    ///     The result value, only meaningful when <see cref="Success" /> is <c>true</c>.
    /// </summary>
    public T? Value { get; }

    private ServiceResult(bool success, string message, T? value)
    {
        Success = success;
        Message = message;
        Value = value;
    }

    /// <summary>
    ///     Creates a successful result with the given value and message.
    /// </summary>
    public static ServiceResult<T> Ok(T value, string message = "")
        => new(true, message, value);

    /// <summary>
    ///     Creates a failed result with the given error message.
    /// </summary>
    public static ServiceResult<T> Fail(string message)
        => new(false, message, default);
}

/// <summary>
///     A non-generic result type for service operations that produce no value.
/// </summary>
public readonly record struct ServiceResult
{
    /// <summary>
    ///     Whether the operation succeeded.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    ///     A human-readable message describing the outcome.
    /// </summary>
    public string Message { get; }

    private ServiceResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    /// <summary>
    ///     Creates a successful result.
    /// </summary>
    public static ServiceResult Ok(string message = "")
        => new(true, message);

    /// <summary>
    ///     Creates a failed result with the given error message.
    /// </summary>
    public static ServiceResult Fail(string message)
        => new(false, message);
}
