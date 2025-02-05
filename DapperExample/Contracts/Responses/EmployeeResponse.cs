namespace DapperExample.Contracts.Responses;

public class EmployeeResponse
{
    public required int Id { get; init; }
    public required string FName { get; init; }
    public required string LName { get; init; }
    public required string Email { get; init; }
    public required string RoleName { get; init; }
}
