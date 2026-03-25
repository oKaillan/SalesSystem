using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Enum;

namespace SalesSystem.API.Services;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
internal class ResultService<T>
{
    /// <summary>
    /// Indicates if action was Success or not
    /// </summary>
    public bool Success { get; private set; }
    /// <summary>
    /// Shows list of errors
    /// </summary>
    public string? Error { get; private set; }
    /// <summary>
    /// Any class
    /// </summary>
    public T? Data { get; private set; }
    /// <summary>
    /// Status Code for Api's (200, 204...)
    /// </summary>
    public ResultStatus Status { get; set; }

    /// <summary>
    /// Returns Ok and data if all goes right
    /// </summary>
    /// <param name="data"></param>
    /// <param name="resultStatus"></param>
    /// <returns></returns>
    public static ResultService<T> Ok(T data, ResultStatus resultStatus) =>
        new ResultService<T> { Success = true, Data = data , Status = resultStatus};
    /// <summary>
    /// Returns Fail errors
    /// </summary>
    /// <param name="error"></param>
    /// <param name="resultStatus"></param>
    /// <returns></returns>
    public static ResultService<T> Fail(string error, ResultStatus resultStatus) =>
        new ResultService<T> { Success = false, Error = error, Status = resultStatus };

}

/// <summary>
/// Converts Result Status response to IActionResult
/// </summary>
internal static class ResultExtensions
{
    /// <summary>
    /// Converts Result Status response to IActionResult
    /// </summary>
    public static IActionResult ToActionResult<T>(this ResultService<T> result)
    {
        return result.Status switch
        {
            ResultStatus.Success => new OkObjectResult(result.Data),
            ResultStatus.NoContent => new NoContentResult(),
            ResultStatus.NotFound => new NotFoundObjectResult(result.Error),
            ResultStatus.BadRequest => new BadRequestObjectResult(result.Error),
            _ => new ObjectResult(result.Error) { StatusCode = 500 }
        };
    }
}