using FluentValidation;

namespace DapperExample.WorkItems;

public class WorkItemValidator : AbstractValidator<WorkItem>
{
    public WorkItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TaskName).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.DueDate).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.StatusId).NotEmpty();
        RuleFor(x => x.Estimate).NotEmpty();
        RuleFor(x => x.PriorityId).NotEmpty();
    }
}
