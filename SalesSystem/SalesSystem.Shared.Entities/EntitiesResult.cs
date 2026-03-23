namespace SalesSystem.Shared.Entities;

public class EntitiesResult
{

    public bool Success { get; set; }
    public string? Error { get; set; }
    
    public EntitiesResult(bool success, string? error)
    {
        Success = success;
        Error = error;
    }
}
