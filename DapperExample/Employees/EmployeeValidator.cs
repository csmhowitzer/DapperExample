using FluentValidation;

namespace DapperExample.Employees;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FName).NotEmpty();
        RuleFor(x => x.LName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
    }
}
