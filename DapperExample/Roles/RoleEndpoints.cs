using DapperExample.Contracts;
using DapperExample.Contracts.Requests;

namespace DapperExample.Roles;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("roles");
        group.MapPost(
            "/",
            async (IRoleService service, CreateRoleRequest request) =>
            {
                var role = request.MaptoRole();
                var result = await service.Create(role);
                return result.Match<IResult>(_ =>
                    Results.CreatedAtRoute(
                        "GetRole",
                        new { id = role.Id },
                        role.MapToResponse()
                    ),
                    failed => Results.BadRequest(failed.MapToResponse())
                );
            }
        );
    }
}
