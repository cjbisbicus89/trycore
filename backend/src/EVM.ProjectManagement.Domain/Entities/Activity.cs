namespace EVM.ProjectManagement.Domain.Entities;

using EVM.ProjectManagement.Domain.Exceptions;

public sealed class Activity
{
    private Activity()
    {
    }

    public Guid Id { get; private set; }

    public Guid ProjectId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public decimal BudgetedCost { get; private set; }

    public decimal PlannedPercentage { get; private set; }

    public decimal ActualPercentage { get; private set; }

    public decimal ActualCost { get; private set; }

#pragma warning disable CA1819
    public byte[] RowVersion { get; private set; } = [];
#pragma warning restore CA1819

    public decimal PlannedValue => BudgetedCost * (PlannedPercentage / ActivityConstants.PercentageDivisor);

    public decimal EarnedValue => BudgetedCost * (ActualPercentage / ActivityConstants.PercentageDivisor);

    public static Activity Create(
        Guid projectId,
        string name,
        decimal budgetedCost,
        decimal plannedPercentage,
        decimal actualPercentage,
        decimal actualCost)
    {
        ValidateActivityData(name, budgetedCost, plannedPercentage, actualPercentage, actualCost);

        return new Activity
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            Name = name,
            BudgetedCost = budgetedCost,
            PlannedPercentage = plannedPercentage,
            ActualPercentage = actualPercentage,
            ActualCost = actualCost,
        };
    }

    public void Update(
        string name,
        decimal budgetedCost,
        decimal plannedPercentage,
        decimal actualPercentage,
        decimal actualCost)
    {
        ValidateActivityData(name, budgetedCost, plannedPercentage, actualPercentage, actualCost);

        Name = name;
        BudgetedCost = budgetedCost;
        PlannedPercentage = plannedPercentage;
        ActualPercentage = actualPercentage;
        ActualCost = actualCost;
    }

    private static void ValidateActivityData(
        string name,
        decimal budgetedCost,
        decimal plannedPercentage,
        decimal actualPercentage,
        decimal actualCost)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(ActivityErrors.NameIsRequired);
        }

        if (budgetedCost < ActivityConstants.MinBudgetedCost)
        {
            throw new DomainException(ActivityErrors.BudgetedCostMustBePositive);
        }

        if (plannedPercentage < ActivityConstants.MinPercentage || plannedPercentage > ActivityConstants.MaxPercentage)
        {
            throw new DomainException(ActivityErrors.PlannedPercentageExceedsMaximum);
        }

        if (actualPercentage < ActivityConstants.MinPercentage || actualPercentage > ActivityConstants.MaxPercentage)
        {
            throw new DomainException(ActivityErrors.ActualPercentageExceedsMaximum);
        }

        if (actualCost < ActivityConstants.MinActualCost)
        {
            throw new DomainException(ActivityErrors.ActualCostCannotBeNegative);
        }
    }
}
