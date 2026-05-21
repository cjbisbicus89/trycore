namespace EVM.ProjectManagement.Domain.Entities;

public sealed class Activity
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal BudgetedCost { get; private set; }
    public decimal PlannedPercentage { get; private set; }
    public decimal ActualPercentage { get; private set; }
    public decimal ActualCost { get; private set; }

    // Propiedades calculadas
    public decimal PlannedValue => BudgetedCost * (PlannedPercentage / 100);
    public decimal EarnedValue => BudgetedCost * (ActualPercentage / 100);

    private Activity() { }

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
            throw new DomainException("Name is required");
        if (budgetedCost <= 0)
            throw new DomainException("BudgetedCost must be greater than zero");
        if (plannedPercentage < 0 || plannedPercentage > 100)
            throw new DomainException("PlannedPercentage must be between 0 and 100");
        if (actualPercentage < 0 || actualPercentage > 100)
            throw new DomainException("ActualPercentage must be between 0 and 100");
        if (actualCost < 0)
            throw new DomainException("ActualCost must be non-negative");

        return new Activity
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            Name = name,
            BudgetedCost = budgetedCost,
            PlannedPercentage = plannedPercentage,
            ActualPercentage = actualPercentage,
            ActualCost = actualCost
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
            throw new DomainException("Name is required");
        if (budgetedCost <= 0)
            throw new DomainException("BudgetedCost must be greater than zero");
        if (plannedPercentage < 0 || plannedPercentage > 100)
            throw new DomainException("PlannedPercentage must be between 0 and 100");
        if (actualPercentage < 0 || actualPercentage > 100)
            throw new DomainException("ActualPercentage must be between 0 and 100");
        if (actualCost < 0)
            throw new DomainException("ActualCost must be non-negative");

        Name = name;
        BudgetedCost = budgetedCost;
        PlannedPercentage = plannedPercentage;
        ActualPercentage = actualPercentage;
        ActualCost = actualCost;
    }
}
