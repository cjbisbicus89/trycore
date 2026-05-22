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
    public void CalculateCostStatusOnBudgetCuandoCPIEsUno()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 1000;
        var actualCost = 1000;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(EVMStatus.OnBudget, indicators.CostStatus);
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
        Assert.Equal(EVMStatus.UnderBudget, indicators.CostStatus);
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
        Assert.Equal(EVMStatus.OverBudget, indicators.CostStatus);
    }

    [Fact]
    public void CalculateScheduleStatusOnScheduleCuandoSPIEsUno()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 1000;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(EVMStatus.OnSchedule, indicators.ScheduleStatus);
    }

    [Fact]
    public void CalculateScheduleStatusAheadOfScheduleCuandoSPIMayorQueUno()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 1200;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(EVMStatus.AheadOfSchedule, indicators.ScheduleStatus);
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
        Assert.Equal(EVMStatus.BehindSchedule, indicators.ScheduleStatus);
    }

    [Fact]
    public void CalculateCVCorrecto()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(-100, indicators.CostVariance);
    }

    [Fact]
    public void CalculateSVCorrecto()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 900;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(-200, indicators.ScheduleVariance);
    }

    [Fact]
    public void CalculateEACAndVACCorrecto()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 800;
        var actualCost = 800;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(1.0m, indicators.CostPerformanceIndex);
        Assert.Equal(2000.0m, indicators.EstimateAtCompletion);
        Assert.Equal(0.0m, indicators.VarianceAtCompletion);
    }

    [Fact]
    public void CalculateEarnedValueZeroCuandoAvanceRealEsCero()
    {
        // Arrange
        var plannedValue = 1000;
        var earnedValue = 0;
        var actualCost = 500;
        var budgetedCost = 2000;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Equal(0, indicators.EarnedValue);
        Assert.Equal(EVMStatus.OverBudget, indicators.CostStatus);
    }

    [Fact]
    public void CalculateAllStatusNotApplicableCuandoNoHayValores()
    {
        // Arrange
        var plannedValue = 0m;
        var earnedValue = 0m;
        var actualCost = 0m;
        var budgetedCost = 0m;

        // Act
        var indicators = this.calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        Assert.Null(indicators.CostPerformanceIndex);
        Assert.Null(indicators.SchedulePerformanceIndex);
        Assert.Equal(EVMStatus.CostNotApplicable, indicators.CostStatus);
        Assert.Equal(EVMStatus.ScheduleNotApplicable, indicators.ScheduleStatus);
    }
}
