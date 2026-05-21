namespace EVM.ProjectManagement.Domain.Services;

using EVM.ProjectManagement.Domain.ValueObjects;

public interface IEVMCalculator
{
    EVMIndicators Calculate(decimal plannedValue, decimal earnedValue, decimal actualCost, decimal budgetedCost);
}
