namespace CleanArchitectureTemplate.Application.Common.Interfaces;

/// <summary>
/// Service to get current time
/// </summary>
public interface IDateTime
{
    DateTime UtcNow { get; }
}
