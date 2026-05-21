using EVM.ProjectManagement.Domain.Entities;

namespace EVM.ProjectManagement.UnitTests.Builders;

public sealed class ActivityBuilder
{
    private Guid _projectId = Guid.NewGuid();
    private string _name = "Test Activity";
    private decimal _budgetedCost = 1000;
    private decimal _plannedPercentage = 50;
    private decimal _actualPercentage = 25;
    private decimal _actualCost = 300;

    public ActivityBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ActivityBuilder WithBudget(decimal budgetedCost)
    {
        _budgetedCost = budgetedCost;
        return this;
    }

    public ActivityBuilder WithPlannedPercentage(decimal plannedPercentage)
    {
        _plannedPercentage = plannedPercentage;
        return this;
    }

    public ActivityBuilder WithActualPercentage(decimal actualPercentage)
    {
        _actualPercentage = actualPercentage;
        return this;
    }

    public ActivityBuilder WithActualCost(decimal actualCost)
    {
        _actualCost = actualCost;
        return this;
    }

    public ActivityBuilder WithProjectId(Guid projectId)
    {
        _projectId = projectId;
        return this;
    }

    public Activity Build()
    {
        return Activity.Create(_projectId, _name, _budgetedCost, _plannedPercentage, _actualPercentage, _actualCost);
    }
}
