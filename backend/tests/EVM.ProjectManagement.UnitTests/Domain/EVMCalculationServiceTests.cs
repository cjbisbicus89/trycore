namespace EVM.ProjectManagement.UnitTests.Domain;

using EVM.ProjectManagement.Domain.Services;
using Xunit;

public sealed class EVMCalculationServiceTests
{
    private readonly EVMCalculationService calculator = new EVMCalculationService();

    [Fact]
    public void CalculateCPICorrectoCuandoACMayorQueCero()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(0.889m, Math.Round(indicators.CostPerformanceIndex!.Value, 3));
    }

    [Fact]
    public void CalculateCPINullCuandoACEsCero()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 0;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Null(indicators.CostPerformanceIndex);
    }

    [Fact]
    public void CalculateSPICorrectoCuandoPVMayorQueCero()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(0.8m, indicators.SchedulePerformanceIndex);
    }

    [Fact]
    public void CalculateSPINullCuandoPVEsCero()
    {
        // Arrange
        var plannedValue = 0;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Null(indicators.SchedulePerformanceIndex);
    }

    [Fact]
    public void CalculateEACNullCuandoCPINull()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 0;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Null(indicators.EstimateAtCompletion);
    }

    [Fact]
    public void CalculateEACNullCuandoCPICero()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 0;
        var actualCost = 1000;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Null(indicators.EstimateAtCompletion);
    }

    [Fact]
    public void CalculateCostStatusUnderBudgetCuandoCPIMayorQueUno()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 900;
        var actualCost = 800;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal("Under Budget", indicators.CostStatus);
    }

    [Fact]
    public void CalculateCostStatusOverBudgetCuandoCPIMenorQueUno()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal("Over Budget", indicators.CostStatus);
    }

    [Fact]
    public void CalculateScheduleStatusBehindScheduleCuandoSPIMenorQueUno()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal("Behind Schedule", indicators.ScheduleStatus);
    }
}
