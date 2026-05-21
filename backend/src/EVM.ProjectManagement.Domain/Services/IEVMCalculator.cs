using EVM.ProjectManagement.Domain.ValueObjects;

namespace EVM.ProjectManagement.Domain.Services;

public interface IEVMCalculator
{
    EVMIndicators Calculate(decimal plannedValue, decimal earnedValue, decimal actualCost, decimal budgetedCost);
}
