namespace DapperExample.Contracts.Requests;

public class UpdateProjectRequest
{
    public required string Name { get; init; }
    public required string Description  { get; init; }
    public required string StartDate { get; init; }
    public required string EndDate { get; init; }
    public required int OwnerId { get; init; }
}

