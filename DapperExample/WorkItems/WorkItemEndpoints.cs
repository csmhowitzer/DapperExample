using DapperExample.Contracts;
using DapperExample.Contracts.Requests;

namespace DapperExample.WorkItems;

public static class WorkItemEndpionts
{
    public static void MapWorkItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("workitems");
        group.MapPost(
            "/",
            async (IWorkItemService service, CreateWorkItemRequest request) =>
            {
                var workItem = request.MapToWorkItem();
                var result = await service.Create(workItem);
                return result.Match<IResult>(_ =>
                    Results.CreatedAtRoute(
                        "GetWorkItem",
                        new { id = workItem.Id },
                        workItem.MapToResponse()
                    ),
                    failed => Results.BadRequest(failed.MapToResponse())
                );
            }
        );
        group.MapGet(
            "id/{id:int}",
            async (IWorkItemService service, int id) =>
            {
                var result = await service.GetById(id);
                return result is not null ? Results.Ok(result.MapToResponse())
                                          : Results.NotFound();
            }
        ).WithName("GetWorkItem");
        group.MapGet(
            "/",
            async (IWorkItemService service) =>
            {
                var workItems = await service.GetAll();
                var response = workItems.MapToResponse();
                return Results.Ok(response);
            }
        );
        group.MapPut(
            "id/{id:int}",
            async (IWorkItemService service, int id, UpdateWorkItemRequest request) =>
            {
                var workItem = request.MapToWorkItem(id);
                var result = await service.Update(workItem);
                return result.Match<IResult>(
                    t => t is not null ? Results.Ok(t.MapToResponse()) : Results.NotFound(),
                    failed => Results.BadRequest(failed.MapToResponse())
                );
            }
        );
        group.MapDelete(
            "id/{id:int}",
            async (IWorkItemService service, int id) =>
            {
                var deleted = await service.DeleteById(id);
                return deleted ? Results.Ok() : Results.NotFound();
            }
        );
    }
}

