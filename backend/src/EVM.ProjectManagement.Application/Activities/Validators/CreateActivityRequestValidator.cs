namespace EVM.ProjectManagement.Application.Activities.Validators;

using EVM.ProjectManagement.Application.Activities.DTOs;
using FluentValidation;

public sealed class CreateActivityRequestValidator : AbstractValidator<CreateActivityRequest>
{
    public CreateActivityRequestValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.BudgetedCost)
            .GreaterThan(0);

        RuleFor(x => x.PlannedPercentage)
            .InclusiveBetween(0, 100);

        RuleFor(x => x.ActualPercentage)
            .InclusiveBetween(0, 100);

        RuleFor(x => x.ActualCost)
            .GreaterThanOrEqualTo(0);
    }
}
