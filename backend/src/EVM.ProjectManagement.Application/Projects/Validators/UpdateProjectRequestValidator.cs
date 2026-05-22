namespace EVM.ProjectManagement.Application.Projects.Validators;

using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Domain.Entities;
using FluentValidation;

public sealed class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ProjectConstants.MaxNameLength);

        this.RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(ProjectConstants.MaxDescriptionLength);
    }
}
