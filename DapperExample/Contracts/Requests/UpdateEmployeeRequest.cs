namespace DapperExample.Contracts.Requests;

public class UpdateEmployeeRequest
{
    public required string FName { get; init; }
    public required string LName { get; init; }
    public required string Email { get; init; }
}
