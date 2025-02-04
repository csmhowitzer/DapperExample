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
        group.MapGet(
            "id/{id:int}",
            async (IRoleService service, int id) =>
            {
                var result = await service.GetById(id);
                return result is not null ? Results.Ok(result.MapToResponse())
                                          : Results.NotFound();
            }
        ).WithName("GetRole");
        group.MapGet(
            "/",
            async (IRoleService service) =>
            {
                var roles = await service.GetAll();
                var response = roles.MapToResponse();
                return Results.Ok(response);
            }
        );
        group.MapPut(
            "id/{id:int}",
            async (IRoleService service, int id, UpdateRoleRequest request) =>
            {
                var role = request.MapToRole(id);
                var result = await service.Update(role);

                return result.Match<IResult>(
                    r => r is not null ? Results.Ok(r.MapToResponse()) : Results.NotFound(),
                    failed => Results.BadRequest(failed.MapToResponse())
                );
            }
        );
        group.MapDelete(
            "id/{id:int}",
            async (IRoleService service, int id) =>
            {
                var deleted = await service.DeleteById(id);
                return deleted ? Results.Ok() : Results.NotFound();
            }
        );
    }
}
