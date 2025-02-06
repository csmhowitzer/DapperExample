using FluentValidation.Results;
using DapperExample.Employees;
using DapperExample.Validation;
using DapperExample.Contracts.Requests;
using DapperExample.Contracts.Responses;
using DapperExample.Roles;
using DapperExample.WorkItems;
using DapperExample.Projects;

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

    public static WorkItem MapToWorkItem(this CreateWorkItemRequest request)
    {
        return new WorkItem
        {
            Id = -1,
            TaskName = request.TaskName,
            Description = request.Description,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId,
            StatusId = request.StatusId,
            Estimate = request.Estimate,
            PriorityId = request.PriorityId
        };
    }

    public static Project MapToProject(this CreateProjectRequest request)
    {
        return new Project
        {
            Id = -1,
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            OwnerId = request.OwnerId
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

    public static WorkItem MapToWorkItem(this UpdateWorkItemRequest request, int id)
    {
        return new WorkItem
        {
            Id = id,
            TaskName = request.TaskName,
            Description = request.Description,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId,
            StatusId = request.StatusId,
            Estimate = request.Estimate,
            PriorityId = request.PriorityId
        };
    }

    public static Project MapToProject(this UpdateProjectRequest request, int id)
    {
        return new Project
        {
            Id = id, 
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            OwnerId = request.OwnerId
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

    public static WorkItemResponse MapToResponse(this WorkItem workItem)
    {
        return new WorkItemResponse
        {
            Id = workItem.Id,
            TaskName = workItem.TaskName,
            Description = workItem.Description,
            StartDate = workItem.StartDate,
            DueDate = workItem.DueDate,
            ProjectId = workItem.ProjectId,
            StatusId = workItem.StatusId,
            Estimate = workItem.Estimate,
            PriorityId = workItem.PriorityId
        };
    }

    public static ProjectResponse MapToResponse(this Project project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Descripttion = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            OwnerId = project.OwnerId
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

    public static WorkItemsResponse MapToResponse(this IEnumerable<WorkItem> workItem)
    {
        return new WorkItemsResponse { Items = workItem.Select(MapToResponse) };
    }

    public static ProjectsResponse MapToResponse(this IEnumerable<Project> projects)
    {
        return new ProjectsResponse { Items = projects.Select(MapToResponse) };
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
