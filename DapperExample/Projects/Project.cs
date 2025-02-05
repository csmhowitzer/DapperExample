namespace DapperExample.Projects;

public class Project
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string StartDate { get; set; }
    public required string EndDate { get; set; }
    public required int OwnerId { get; set; }
}
