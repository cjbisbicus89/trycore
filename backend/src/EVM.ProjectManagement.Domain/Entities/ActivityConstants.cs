namespace EVM.ProjectManagement.Domain.Entities;

public static class ActivityConstants
{
    public const decimal PercentageDivisor = 100m;
    public const decimal MinPercentage = 0m;
    public const decimal MaxPercentage = 100m;
    public const decimal MinBudgetedCost = 0.01m;
    public const decimal MinActualCost = 0m;
    public const int MaxNameLength = 200;
    public const int BudgetedCostPrecision = 18;
    public const int BudgetedCostScale = 2;
    public const int PercentagePrecision = 5;
    public const int PercentageScale = 2;
}
