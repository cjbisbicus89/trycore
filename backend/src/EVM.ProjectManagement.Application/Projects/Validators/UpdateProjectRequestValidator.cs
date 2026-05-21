namespace EVM.ProjectManagement.Application.Projects.Validators;

using EVM.ProjectManagement.Application.Projects.DTOs;
using FluentValidation;

public sealed class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        this.RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
