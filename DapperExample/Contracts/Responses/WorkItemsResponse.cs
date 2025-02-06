namespace DapperExample.Contracts.Responses;

public class WorkItemsResponse
{
    public required IEnumerable<WorkItemResponse> Items { get; init; } = 
        Enumerable.Empty<WorkItemResponse>();
}
