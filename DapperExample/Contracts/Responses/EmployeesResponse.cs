namespace DapperExample.Contracts.Responses;

public class EmployeesResponse
{
    public required IEnumerable<EmployeeResponse> Items { get; init; } = [];
}
