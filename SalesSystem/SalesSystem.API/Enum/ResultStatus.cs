namespace SalesSystem.API.Enum;

/// <summary>
/// Return status codes for API's (200, 204...)
/// </summary>
internal enum ResultStatus
{
    /// <summary>
    /// Indicates Ok (200)
    /// </summary>
    Success,
    /// <summary>
    /// Indicates NotFound(404)
    /// </summary>
    NotFound,
    /// <summary>
    /// Indicates BadRequest(400)
    /// </summary>
    BadRequest,
    /// <summary>
    /// Indicates NoContent (204)
    /// </summary>
    NoContent,
    /// <summary>
    /// Shows all errors
    /// </summary>
    Error
}
