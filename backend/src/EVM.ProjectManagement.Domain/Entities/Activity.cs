namespace EVM.ProjectManagement.Domain.Entities;

using EVM.ProjectManagement.Domain.Exceptions;

public sealed class Activity
{
    private const decimal PercentageDivisor = 100m;
    private const decimal MinPercentage = 0m;
    private const decimal MaxPercentage = 100m;

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

    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    // Propiedades calculadas
    public decimal PlannedValue => this.BudgetedCost * (this.PlannedPercentage / PercentageDivisor);

    public decimal EarnedValue => this.BudgetedCost * (this.ActualPercentage / PercentageDivisor);

    public static Activity Create(
        Guid projectId,
        string name,
        decimal budgetedCost,
        decimal plannedPercentage,
        decimal actualPercentage,
        decimal actualCost)
    {
        // Validación de invariantes
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name is required");
        }

        if (budgetedCost <= 0)
        {
            throw new DomainException("BudgetedCost must be greater than zero");
        }

        if (plannedPercentage < MinPercentage || plannedPercentage > MaxPercentage)
        {
            throw new DomainException($"PlannedPercentage must be between {MinPercentage} and {MaxPercentage}");
        }

        if (actualPercentage < MinPercentage || actualPercentage > MaxPercentage)
        {
            throw new DomainException($"ActualPercentage must be between {MinPercentage} and {MaxPercentage}");
        }

        if (actualCost < 0)
        {
            throw new DomainException("ActualCost must be non-negative");
        }

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
        // Validación de invariantes
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name is required");
        }

        if (budgetedCost <= 0)
        {
            throw new DomainException("BudgetedCost must be greater than zero");
        }

        if (plannedPercentage < MinPercentage || plannedPercentage > MaxPercentage)
        {
            throw new DomainException($"PlannedPercentage must be between {MinPercentage} and {MaxPercentage}");
        }

        if (actualPercentage < MinPercentage || actualPercentage > MaxPercentage)
        {
            throw new DomainException($"ActualPercentage must be between {MinPercentage} and {MaxPercentage}");
        }

        if (actualCost < 0)
        {
            throw new DomainException("ActualCost must be non-negative");
        }

        this.Name = name;
        this.BudgetedCost = budgetedCost;
        this.PlannedPercentage = plannedPercentage;
        this.ActualPercentage = actualPercentage;
        this.ActualCost = actualCost;
    }
}
