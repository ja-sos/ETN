using System.Net;

namespace ETN.Application.Contracts;

/// <summary>
/// Defines the contract for operation results.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccessful { get; init; }

    /// <summary>
    /// Collection of messages related to the operation.
    /// </summary>
    public IReadOnlyList<string> Messages { get; init; }

    /// <summary>
    /// The HTTP status code representing the result of the operation.
    /// </summary>
    public HttpStatusCode StatusCode { get; init; }
}

/// <summary>
/// Defines the contract for operation results with data.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResult<T> : IResult
{
    /// <summary>
    /// The data returned by the operation.
    /// </summary>
    public T? Data { get; init; }
}
