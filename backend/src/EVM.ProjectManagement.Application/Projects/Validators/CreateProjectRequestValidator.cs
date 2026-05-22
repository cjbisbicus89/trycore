namespace EVM.ProjectManagement.Application.Projects.Validators;

using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Domain.Entities;
using FluentValidation;

public sealed class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ProjectConstants.MaxNameLength);

        this.RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(ProjectConstants.MaxDescriptionLength);
    }
}
