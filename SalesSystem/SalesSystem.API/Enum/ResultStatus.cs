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
    /// Indicates Created (201)
    /// </summary>
    Created,
    /// <summary>
    /// Indicates Not Found (404)
    /// </summary>
    NotFound,
    /// <summary>
    /// Indicates Bad Request (400)
    /// </summary>
    BadRequest,
    /// <summary>
    /// Indicates No Content (204)
    /// </summary>
    NoContent,
    /// <summary>
    /// Indicates Conflict (409)
    /// </summary>
    Conflict,
    Error
}
