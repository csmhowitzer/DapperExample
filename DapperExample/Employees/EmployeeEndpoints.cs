using DapperExample.Contracts;
using DapperExample.Contracts.Requests;

namespace DapperExample.Employees;

// an alternative way to inheriting from API Controller
public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("employees");
        group.MapPost(
            "/",
            async (IEmployeeService service, CreateEmployeeRequest request) =>
            {
                var employee = request.MapToEmployee();
                var result = await service.Create(employee);
                return result.Match<IResult>(
                    _ =>
                        Results.CreatedAtRoute(
                            "GetEmployee",
                            new { id = employee.Id },
                            employee.MapToResponse()
                        ),
                    failed => Results.BadRequest(failed.MapToResponse())
                );
            }
        );
        group
            .MapGet(
                "/{id:int}",
                async (IEmployeeService service, int id) =>
                {
                    var result = await service.GetById(id);
                    return result is not null
                        ? Results.Ok(result.MapToResponse())
                        : Results.NotFound();
                }
            )
            .WithName("GetEmployee");
        group.MapGet(
            "/",
            async (IEmployeeService service) =>
            {
                var employees = await service.GetAll();
                var response = employees.MapToResponse();
                return Results.Ok(response);
            }
        );
        group.MapPut(
            "/{id:int}",
            async (IEmployeeService service, int id, UpdateEmployeeRequest request) =>
            {
                var employee = request.MapToEmployee(id);
                var result = await service.Update(employee);

                return result.Match<IResult>(
                    e => e is not null ? Results.Ok(e.MapToResponse()) : Results.NotFound(),
                    failed => Results.BadRequest(failed.MapToResponse())
                );
            }
        );
        group.MapDelete(
            "/{id:int}",
            async (IEmployeeService service, int id) =>
            {
                var deleted = await service.DeleteById(id);
                return deleted ? Results.Ok() : Results.NotFound();
            }
        );
    }
}
