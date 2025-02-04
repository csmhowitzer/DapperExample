namespace DapperExample.Contracts.Requests;

public class UpdateRoleRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}
