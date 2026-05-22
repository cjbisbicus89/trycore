namespace EVM.ProjectManagement.Application.Activities.Validators;

using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Domain.Entities;
using FluentValidation;

public sealed class UpdateActivityRequestValidator : AbstractValidator<UpdateActivityRequest>
{
    public UpdateActivityRequestValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ActivityConstants.MaxNameLength);

        this.RuleFor(x => x.BudgetedCost)
            .GreaterThan(ActivityConstants.MinBudgetedCost);

        this.RuleFor(x => x.PlannedPercentage)
            .InclusiveBetween(ActivityConstants.MinPercentage, ActivityConstants.MaxPercentage);

        this.RuleFor(x => x.ActualPercentage)
            .InclusiveBetween(ActivityConstants.MinPercentage, ActivityConstants.MaxPercentage);

        this.RuleFor(x => x.ActualCost)
            .GreaterThanOrEqualTo(ActivityConstants.MinActualCost);
    }
}
