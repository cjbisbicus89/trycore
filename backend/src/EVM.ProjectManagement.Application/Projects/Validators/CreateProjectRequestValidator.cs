namespace EVM.ProjectManagement.Application.Projects.Validators;

using EVM.ProjectManagement.Application.Projects.DTOs;
using FluentValidation;

public sealed class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
