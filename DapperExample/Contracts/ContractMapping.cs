using FluentValidation.Results;
using DapperExample.Employees;
using DapperExample.Validation;
using DapperExample.Contracts.Requests;
using DapperExample.Contracts.Responses;
using System.Linq;
using DapperExample.Roles;

namespace DapperExample.Contracts;

public static class ContractMapping
{
    public static Employee MapToEmployee(this CreateEmployeeRequest request)
    {
        return new Employee
        {
            Id = -1,
            FName = request.FName,
            LName = request.LName,
            Email = request.Email,
            RoleName = request.RoleName
        };
    }

    public static Role MaptoRole(this CreateRoleRequest request)
    {
        return new Role
        {
            Id = -1,
            Name = request.Name,
            Desc = request.Desc
        };
    }

    public static Employee MapToEmployee(this UpdateEmployeeRequest request, int id)
    {
        return new Employee
        {
            Id = id,
            FName = request.FName,
            LName = request.LName,
            Email = request.Email,
            RoleName = request.RoleName
        };
    }

    public static Role MapToRole(this UpdateRoleRequest request, int id)
    {
        return new Role
        {
            Id = id,
            Name = request.Name,
            Desc = request.Desc
        };
    }

    public static EmployeeResponse MapToResponse(this Employee employee)
    {
        return new EmployeeResponse
        {
            Id = employee.Id,
            FName = employee.FName,
            LName = employee.LName,
            Email = employee.Email,
            RoleName = employee.RoleName
        };
    }

    public static RoleResponse MapToResponse(this Role role)
    {
        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name,
            Desc = role.Desc
        };
    }

    public static EmployeesResponse MapToResponse(this IEnumerable<Employee> employees)
    {
        return new EmployeesResponse { Items = employees.Select(MapToResponse) };
    }

    public static RolesResponse MapToResponse(this IEnumerable<Role> roles)
    {
        return new RolesResponse { Items = roles.Select(MapToResponse) };
    }

    public static ValidationFailureResponse MapToResponse(this IEnumerable<ValidationFailure> validationFailures)
    {
        return new ValidationFailureResponse
        {
            Errors = validationFailures.Select(x => new ValidationResponse
            {
                PropertyName = x.PropertyName,
                Message = x.ErrorMessage,
            }),
        };
    }

    public static ValidationFailureResponse MapToResponse(this ValidationFailed failed)
    {
        return new ValidationFailureResponse
        {
            Errors = failed.Errors.Select(x => new ValidationResponse
            {
                PropertyName = x.PropertyName,
                Message = x.ErrorMessage,
            }),
        };
    }
}
