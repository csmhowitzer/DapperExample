namespace DapperExample.Employees;

public class Employee
{
    public int Id { get; init; }
    public required string FName { get; set; }
    public required string LName { get; set; }
    public required string Email { get; set; }
    public required string RoleName { get; set; }
}
