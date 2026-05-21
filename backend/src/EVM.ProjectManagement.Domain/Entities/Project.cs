namespace EVM.ProjectManagement.Domain.Entities;

public sealed class Project
{
    private readonly List<Activity> _activities = [];

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<Activity> Activities => _activities.AsReadOnly();

    // Propiedades calculadas sobre la colección de actividades
    public decimal TotalPlannedValue => _activities.Sum(a => a.PlannedValue);
    public decimal TotalEarnedValue => _activities.Sum(a => a.EarnedValue);
    public decimal TotalActualCost => _activities.Sum(a => a.ActualCost);
    public decimal TotalBudgetedCost => _activities.Sum(a => a.BudgetedCost);

    private Project() { }

    public static Project Create(string name, string description)
    {
        // Validación de invariantes
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required");
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description is required");

        return new Project
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string description)
    {
        // Validación de invariantes
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is required");
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description is required");

        Name = name;
        Description = description;
    }

    public void SetActivities(IEnumerable<Activity> activities)
    {
        _activities.Clear();
        _activities.AddRange(activities);
    }
}
