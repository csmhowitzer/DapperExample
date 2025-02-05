namespace DapperExample.WorkItems;

public class WorkItem
{
    public required int Id { get; init; }
    public required string TaskName { get; set; }
    public required string Description { get; set; }
    public required string StartDate { get; set; }
    public required string DueDate { get; set; }
    public required int ProjectId { get; set; }
    public required int StatusId { get; set; }
    public required int Estimate { get; set; }
    public required int PriorityId { get; set; }
}


