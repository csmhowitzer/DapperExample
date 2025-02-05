namespace DapperExample.Contracts.Responses;

public class ProjectResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Descripttion { get; init; }
    public required string StartDate { get; init; }
    public required string EndDate { get; init; }
    public required int OwnerId { get; init; }
}
