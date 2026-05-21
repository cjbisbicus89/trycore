namespace EVM.ProjectManagement.Application.Activities.Validators;

using EVM.ProjectManagement.Application.Activities.DTOs;
using FluentValidation;

public sealed class UpdateActivityRequestValidator : AbstractValidator<UpdateActivityRequest>
{
    public UpdateActivityRequestValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        this.RuleFor(x => x.BudgetedCost)
            .GreaterThan(0);

        this.RuleFor(x => x.PlannedPercentage)
            .InclusiveBetween(0, 100);

        this.RuleFor(x => x.ActualPercentage)
            .InclusiveBetween(0, 100);

        this.RuleFor(x => x.ActualCost)
            .GreaterThanOrEqualTo(0);
    }
}
