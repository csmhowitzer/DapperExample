namespace DapperExample.Contracts.Responses;

public class RolesResponse
{
    public required IEnumerable<RoleResponse> Items { get; init; } =
        Enumerable.Empty<RoleResponse>();
}
