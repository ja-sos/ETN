using System.Net;
using ETN.Application.Contracts;

namespace ETN.Application.Results;

/// <summary>
/// Implements the result of an operation.
/// </summary>
public class Result : IResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccessful">Indicates whether the operation was successful. Defaults to true.</param>
    /// <param name="statusCode">The HTTP status code representing the result of the operation. Defaults to OK.</param>
    public Result(bool isSuccessful = true, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        IsSuccessful = isSuccessful;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class with a message.
    /// </summary>
    /// <param name="message">Message associated with the result.</param>
    /// <param name="isSuccessful">Indicates whether the operation was successful. Defaults to false.</param>
    /// <param name="statusCode">The HTTP status code representing the result of the operation. Defaults to BadRequest.</param>
    public Result(string message, bool isSuccessful = false, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Messages = [message];
        IsSuccessful = isSuccessful;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class with multiple messages.
    /// </summary>
    /// <param name="messages">Collection of messages associated with the result.</param>
    /// <param name="isSuccessful">Indicates whether the operation was successful. Defaults to false.</param>
    /// <param name="statusCode">The HTTP status code representing the result of the operation. Defaults to BadRequest.</param>
    public Result(IReadOnlyList<string> messages, bool isSuccessful = false, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Messages = messages;
        IsSuccessful = isSuccessful;
        StatusCode = statusCode;
    }

    /// <inheritdoc/>
    public bool IsSuccessful { get; init; }

    /// <inheritdoc/>
    public IReadOnlyList<string> Messages { get; init; } = [];

    /// <inheritdoc/>
    public HttpStatusCode StatusCode { get; init; }
}

/// <summary>
/// Implements the result of an operation with data.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Result<T> : IResult<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with data.
    /// </summary>
    /// <param name="data">The data returned by the operation.</param>
    /// <param name="isSuccessful">Indicates whether the operation was successful. Defaults to true.</param>
    /// <param name="statusCode">The HTTP status code representing the result of the operation. Defaults to OK.</param>
    public Result(T data, bool isSuccessful = true, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        Data = data;
        IsSuccessful = isSuccessful;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with a message.
    /// </summary>
    /// <param name="message">Message associated with the result.</param>
    /// <param name="isSuccessful">Indicates whether the operation was successful. Defaults to false.</param>
    /// <param name="statusCode">The HTTP status code representing the result of the operation. Defaults to BadRequest.</param>
    public Result(string message, bool isSuccessful = false, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Messages = [message];
        IsSuccessful = isSuccessful;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with multiple messages.
    /// </summary>
    /// <param name="messages">Collection of messages associated with the result.</param>
    /// <param name="isSuccessful">Indicates whether the operation was successful. Defaults to false.</param>
    /// <param name="statusCode">The HTTP status code representing the result of the operation. Defaults to BadRequest.</param>
    public Result(IReadOnlyList<string> messages, bool isSuccessful = false, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Messages = messages;
        IsSuccessful = isSuccessful;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with data and a message.
    /// </summary>
    /// <param name="data">The data returned by the operation.</param>
    /// <param name="message">Message associated with the result.</param>
    /// <param name="isSuccessful">Indicates whether the operation was successful. Defaults to false.</param>
    /// <param name="statusCode">The HTTP status code representing the result of the operation. Defaults to BadRequest.</param>"
    public Result(T data, string message, bool isSuccessful = false, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Data = data;
        Messages = [message];
        IsSuccessful = isSuccessful;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class with data and multiple messages.
    /// </summary>
    /// <param name="data">The data returned by the operation.</param>
    /// <param name="messages">Collection of messages associated with the result.</param>
    /// <param name="isSuccessful">Indicates whether the operation was successful. Defaults to false.</param>
    /// <param name="statusCode">The HTTP status code representing the result of the operation. Defaults to BadRequest.</param>"
    public Result(T data, IReadOnlyList<string> messages, bool isSuccessful = false, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        Data = data;
        Messages = messages;
        IsSuccessful = isSuccessful;
        StatusCode = statusCode;
    }

    /// <inheritdoc/>
    public bool IsSuccessful { get; init; }

    /// <inheritdoc/>
    public IReadOnlyList<string> Messages { get; init; } = [];

    /// <inheritdoc/>
    public T? Data { get; init; }

    /// <inheritdoc/>
    public HttpStatusCode StatusCode { get; init; }
}
