namespace EVM.ProjectManagement.Domain.Entities;

using EVM.ProjectManagement.Domain.Exceptions;

public sealed class Project
{
    private readonly List<Activity> activities = [];

    private Project()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<Activity> Activities => this.activities.AsReadOnly();

    // Propiedades calculadas sobre la colección de actividades
    public decimal TotalPlannedValue => this.activities.Sum(a => a.PlannedValue);

    public decimal TotalEarnedValue => this.activities.Sum(a => a.EarnedValue);

    public decimal TotalActualCost => this.activities.Sum(a => a.ActualCost);

    public decimal TotalBudgetedCost => this.activities.Sum(a => a.BudgetedCost);

    public static Project Create(string name, string description)
    {
        // Validación de invariantes
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name is required");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Description is required");
        }

        return new Project
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public void Update(string name, string description)
    {
        // Validación de invariantes
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name is required");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Description is required");
        }

        this.Name = name;
        this.Description = description;
    }

    public void SetActivities(IEnumerable<Activity> activities)
    {
        this.activities.Clear();
        this.activities.AddRange(activities);
    }
}
