namespace EVM.ProjectManagement.UnitTests.Builders;

using EVM.ProjectManagement.Domain.Entities;

public sealed class ActivityBuilder
{
    private Guid projectId = Guid.NewGuid();
    private string name = "Test Activity";
    private decimal budgetedCost = 1000;
    private decimal plannedPercentage = 50;
    private decimal actualPercentage = 25;
    private decimal actualCost = 300;

    public ActivityBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }

    public ActivityBuilder WithBudget(decimal budgetedCost)
    {
        this.budgetedCost = budgetedCost;
        return this;
    }

    public ActivityBuilder WithPlannedPercentage(decimal plannedPercentage)
    {
        this.plannedPercentage = plannedPercentage;
        return this;
    }

    public ActivityBuilder WithActualPercentage(decimal actualPercentage)
    {
        this.actualPercentage = actualPercentage;
        return this;
    }

    public ActivityBuilder WithActualCost(decimal actualCost)
    {
        this.actualCost = actualCost;
        return this;
    }

    public ActivityBuilder WithProjectId(Guid projectId)
    {
        this.projectId = projectId;
        return this;
    }

    public Activity Build()
    {
        return Activity.Create(this.projectId, this.name, this.budgetedCost, this.plannedPercentage, this.actualPercentage, this.actualCost);
    }
}
