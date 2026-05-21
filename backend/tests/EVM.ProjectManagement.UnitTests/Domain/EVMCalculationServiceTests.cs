using EVM.ProjectManagement.Domain.Services;
using Xunit;

namespace EVM.ProjectManagement.UnitTests.Domain;

public sealed class EVMCalculationServiceTests
{
    private readonly IEVMCalculator _calculator = new EVMCalculationService();

    [Fact]
    public void Calculate_CPI_Correcto_Cuando_AC_Mayor_Que_Cero()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(0.888m, Math.Round(indicators.CostPerformanceIndex!.Value, 3));
    }

    [Fact]
    public void Calculate_CPI_Null_Cuando_AC_Es_Cero()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 0;
        var budgetedCost = 2000;

        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Null(indicators.CostPerformanceIndex);
    }

    [Fact]
    public void Calculate_SPI_Correcto_Cuando_PV_Mayor_Que_Cero()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(0.8m, indicators.SchedulePerformanceIndex);
    }

    [Fact]
    public void Calculate_SPI_Null_Cuando_PV_Es_Cero()
    {
        // Arrange
        var plannedValue = 0;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Null(indicators.SchedulePerformanceIndex);
    }

    [Fact]
    public void Calculate_EAC_Null_Cuando_CPI_Null()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 0;
        var budgetedCost = 2000;

        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Null(indicators.EstimateAtCompletion);
    }

    [Fact]
    public void Calculate_EAC_Null_Cuando_CPI_Cero()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 0;
        var actualCost = 1000;
        var budgetedCost = 2000;

        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Null(indicators.EstimateAtCompletion);
    }

    [Fact]
    public void Calculate_CostStatus_UnderBudget_Cuando_CPI_Mayor_Que_Uno()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 900;
        var actualCost = 800;
        var budgetedCost = 2000;

        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal("Under Budget", indicators.CostStatus);
    }

    [Fact]
    public void Calculate_CostStatus_OverBudget_Cuando_CPI_Menor_Que_Uno()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal("Over Budget", indicators.CostStatus);
    }

    [Fact]
    public void Calculate_ScheduleStatus_BehindSchedule_Cuando_SPI_Menor_Que_Uno()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal("Behind Schedule", indicators.ScheduleStatus);
    }
}
