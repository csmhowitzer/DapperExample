using FluentValidation.Results;
using DapperExample.Employees;
using DapperExample.Validation;
using DapperExample.Contracts.Requests;
using DapperExample.Contracts.Responses;
using System.Linq;

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
            Email = request.Email
        };
    }

    public static Employee MapToEmployee(this UpdateEmployeeRequest request, int id)
    {
        return new Employee
        {
            Id = id,
            FName = request.FName,
            LName = request.LName,
            Email = request.Email
        };
    }

    public static EmployeeResponse MapToResponse(this Employee employee)
    {
        return new EmployeeResponse
        {
            Id = employee.Id,
            FName = employee.FName,
            LName = employee.LName,
            Email = employee.Email
        };
    }

    public static EmployeesResponse MapToResponse(this IEnumerable<Employee> employees)
    {
        return new EmployeesResponse { Items = employees.Select(MapToResponse) };
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
