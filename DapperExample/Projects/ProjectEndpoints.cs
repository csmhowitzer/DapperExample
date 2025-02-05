using DapperExample.Contracts;
using DapperExample.Contracts.Requests;

namespace DapperExample.Projects;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("projects");
        group.MapPost(
            "/",
            async (IProjectService service, CreateProjectRequest request) =>
            {
                var project = request.MapToProject();
                var result = await service.Create(project); 
                return result.Match<IResult>(_ =>
                    Results.CreatedAtRoute(
                        "GetProject",
                        new { id = project.Id },
                        project.MapToResponse()
                    ),
                    failed => Results.BadRequest(failed.MapToResponse())
                );
            }
        );
        group.MapGet(
            "id/{id:int}",
            async (IProjectService service, int id) =>
            {
                var result = await service.GetById(id);
                return result is not null ? Results.Ok(result.MapToResponse())
                                          : Results.NotFound();
            }
        ).WithName("GetProject");
        group.MapGet(
            "/",
            async (IProjectService service) =>
            {
                var projects = await service.GetAll();
                var response = projects.MapToResponse();
                return Results.Ok(response);
            }
        );
        group.MapPut(
            "id/{id:int}",
            async (IProjectService service, int id, UpdateProjectRequest request) =>
            {
                var project = request.MapToProject(id);
                var result = await service.Update(project);
                return result.Match<IResult>(
                    p => p is not null ? Results.Ok(p.MapToResponse()) : Results.NotFound(),
                    failed => Results.BadRequest(failed.MapToResponse())
                );
            }
        );
        group.MapDelete(
            "id/{id:int}",
            async (IProjectService service, int id) =>
            {
                var deleted = await service.DeleteById(id);
                return deleted ? Results.Ok() : Results.NotFound();
            }
        );
    }

}
