using DapperExample.WorkItems;

namespace DapperExample.Projects;

public class ProjectTask
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<WorkItem> WorkItems { get; set; }
}
