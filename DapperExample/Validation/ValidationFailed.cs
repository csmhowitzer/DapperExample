using FluentValidation.Results;

namespace DapperExample.Validation;

public record ValidationFailed(IEnumerable<ValidationFailure> Errors)
{
    public ValidationFailed(ValidationFailure error) : this(new[] { error }) { }
}
