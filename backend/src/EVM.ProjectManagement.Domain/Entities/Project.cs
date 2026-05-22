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

#pragma warning disable CA1819
    public byte[] RowVersion { get; private set; } = [];
#pragma warning restore CA1819

    public IReadOnlyCollection<Activity> Activities => activities.AsReadOnly();

    public decimal TotalPlannedValue => activities.Sum(a => a.PlannedValue);

    public decimal TotalEarnedValue => activities.Sum(a => a.EarnedValue);

    public decimal TotalActualCost => activities.Sum(a => a.ActualCost);

    public decimal TotalBudgetedCost => activities.Sum(a => a.BudgetedCost);

    public static Project Create(string name, string description)
    {
        ValidateProjectData(name, description);

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
        ValidateProjectData(name, description);

        Name = name;
        Description = description;
    }

    public void SetActivities(IEnumerable<Activity> activities)
    {
        this.activities.Clear();
        this.activities.AddRange(activities);
    }

    private static void ValidateProjectData(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(ProjectErrors.NameIsRequired);
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException(ProjectErrors.DescriptionIsRequired);
        }
    }
}
