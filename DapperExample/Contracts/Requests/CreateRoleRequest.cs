namespace DapperExample.Contracts.Requests;

public class CreateRoleRequest
{
    public required string Name { get; init; }
    public required string Desc { get; init; }
}
