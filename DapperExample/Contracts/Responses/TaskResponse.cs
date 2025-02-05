namespace DapperExample.Contracts.Responses;

public class TaskResponse
{
    public required int Id { get; init; }
    public required string TaskName { get; init; }
    public required string Description { get; init; }
    public required string StartDate { get; init; }
    public required string DueDate { get; init; }
    public required int ProjectId { get; init; }
    public required int StatusId { get; init; }
    public required int Estimate { get; init; }
    public required int PriorityId { get; init; }
}

