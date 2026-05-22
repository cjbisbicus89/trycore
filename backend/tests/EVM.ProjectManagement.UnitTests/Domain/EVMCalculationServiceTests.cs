namespace EVM.ProjectManagement.UnitTests.Domain;

using EVM.ProjectManagement.Domain.Services;
using FluentAssertions;
using Xunit;

public sealed class EVMCalculationServiceTests
{
    private readonly EVMCalculationService _calculator = new();

    [Theory]
    [InlineData(1000, 800, 900, 2000, 0.889)]
    [InlineData(500, 400, 450, 1000, 0.889)]
    [InlineData(2000, 1500, 1600, 3000, 0.938)]
    public void CalculateCPI_WhenActualCostIsGreaterThanZero_ReturnsCorrectValue(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost,
        decimal expectedCpi)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.CostPerformanceIndex.Should().NotBeNull();
        indicators.CostPerformanceIndex.Should().BeApproximately(expectedCpi, 0.001m);
    }

    [Theory]
    [InlineData(1000, 800, 0, 2000)]
    [InlineData(500, 400, 0, 1000)]
    public void CalculateCPI_WhenActualCostIsZero_ReturnsNull(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.CostPerformanceIndex.Should().BeNull();
    }

    [Theory]
    [InlineData(1000, 800, 900, 2000, 0.8)]
    [InlineData(500, 400, 450, 1000, 0.8)]
    [InlineData(2000, 1800, 1600, 3000, 0.9)]
    public void CalculateSPI_WhenPlannedValueIsGreaterThanZero_ReturnsCorrectValue(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost,
        decimal expectedSpi)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.SchedulePerformanceIndex.Should().NotBeNull();
        indicators.SchedulePerformanceIndex.Should().Be(expectedSpi);
    }

    [Theory]
    [InlineData(0, 800, 900, 2000)]
    [InlineData(0, 400, 450, 1000)]
    public void CalculateSPI_WhenPlannedValueIsZero_ReturnsNull(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.SchedulePerformanceIndex.Should().BeNull();
    }

    [Theory]
    [InlineData(1000, 800, 0, 2000)]
    [InlineData(500, 400, 0, 1000)]
    public void CalculateEAC_WhenCPIIsNull_ReturnsNull(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.EstimateAtCompletion.Should().BeNull();
        indicators.VarianceAtCompletion.Should().BeNull();
    }

    [Theory]
    [InlineData(1000, 0, 1000, 2000)]
    [InlineData(500, 0, 500, 1000)]
    public void CalculateEAC_WhenCPIIsZero_ReturnsNull(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.EstimateAtCompletion.Should().BeNull();
        indicators.VarianceAtCompletion.Should().BeNull();
    }

    [Theory]
    [InlineData(1000, 1000, 1000, 2000)]
    [InlineData(500, 500, 500, 1000)]
    public void CalculateCostStatus_WhenCPIIsOne_ReturnsOnBudget(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.CostStatus.Should().Be(EVMStatus.EnPresupuesto);
    }

    [Theory]
    [InlineData(1000, 900, 800, 2000)]
    [InlineData(500, 450, 400, 1000)]
    public void CalculateCostStatus_WhenCPIIsGreaterThanOne_ReturnsUnderBudget(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.CostStatus.Should().Be(EVMStatus.BajoPresupuesto);
    }

    [Theory]
    [InlineData(1000, 800, 900, 2000)]
    [InlineData(500, 400, 450, 1000)]
    public void CalculateCostStatus_WhenCPIIsLessThanOne_ReturnsOverBudget(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.CostStatus.Should().Be(EVMStatus.SobrePresupuesto);
    }

    [Theory]
    [InlineData(1000, 1000, 900, 2000)]
    [InlineData(500, 500, 450, 1000)]
    public void CalculateScheduleStatus_WhenSPIIsOne_ReturnsOnSchedule(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.ScheduleStatus.Should().Be(EVMStatus.EnCronograma);
    }

    [Theory]
    [InlineData(1000, 1200, 900, 2000)]
    [InlineData(500, 600, 450, 1000)]
    public void CalculateScheduleStatus_WhenSPIIsGreaterThanOne_ReturnsAheadOfSchedule(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.ScheduleStatus.Should().Be(EVMStatus.AdelantadoCronograma);
    }

    [Theory]
    [InlineData(1000, 800, 900, 2000)]
    [InlineData(500, 400, 450, 1000)]
    public void CalculateScheduleStatus_WhenSPIIsLessThanOne_ReturnsBehindSchedule(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.ScheduleStatus.Should().Be(EVMStatus.AtrasadoCronograma);
    }

    [Theory]
    [InlineData(1000, 800, 900, 2000, -100)]
    [InlineData(500, 400, 450, 1000, -50)]
    [InlineData(1000, 1100, 900, 2000, 200)]
    public void CalculateCV_WhenCalculated_ReturnsCorrectValue(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost,
        decimal expectedCv)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.CostVariance.Should().Be(expectedCv);
    }

    [Theory]
    [InlineData(1000, 800, 900, 2000, -200)]
    [InlineData(500, 400, 450, 1000, -100)]
    [InlineData(1000, 1200, 900, 2000, 200)]
    public void CalculateSV_WhenCalculated_ReturnsCorrectValue(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost,
        decimal expectedSv)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.ScheduleVariance.Should().Be(expectedSv);
    }

    [Theory]
    [InlineData(1000, 800, 800, 2000, 1.0, 2000.0, 0.0)]
    [InlineData(500, 400, 400, 1000, 1.0, 1000.0, 0.0)]
    public void CalculateEACAndVAC_WhenCalculated_ReturnsCorrectValues(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost,
        decimal expectedCpi,
        decimal expectedEac,
        decimal expectedVac)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.CostPerformanceIndex.Should().Be(expectedCpi);
        indicators.EstimateAtCompletion.Should().Be(expectedEac);
        indicators.VarianceAtCompletion.Should().Be(expectedVac);
    }

    [Theory]
    [InlineData(1000, 0, 500, 2000, 0, -500, -1000)]
    [InlineData(500, 0, 250, 1000, 0, -250, -500)]
    public void CalculateEarnedValue_WhenActualProgressIsZero_ReturnsZeroAndCalculatesCVAndSV(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost,
        decimal expectedEv,
        decimal expectedCv,
        decimal expectedSv)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.EarnedValue.Should().Be(expectedEv);
        indicators.CostVariance.Should().Be(expectedCv);
        indicators.ScheduleVariance.Should().Be(expectedSv);
        indicators.CostStatus.Should().Be(EVMStatus.SobrePresupuesto);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0, 0, 0, 1000)]
    public void CalculateAllStatus_WhenNoValues_ReturnsNotApplicable(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.CostPerformanceIndex.Should().BeNull();
        indicators.SchedulePerformanceIndex.Should().BeNull();
        indicators.EstimateAtCompletion.Should().BeNull();
        indicators.VarianceAtCompletion.Should().BeNull();
        indicators.CostStatus.Should().Be(EVMStatus.CostoNoAplicable);
        indicators.ScheduleStatus.Should().Be(EVMStatus.CronogramaNoAplicable);
    }

    [Theory]
    [InlineData(0, 0, 500, 0)]
    [InlineData(0, 0, 1000, 0)]
    public void Calculate_WhenBACIsZero_ReturnsZeroForEvAndPvButNullForEacAndVac(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Act
        var indicators = _calculator.Calculate(plannedValue, earnedValue, actualCost, budgetedCost);

        // Assert
        indicators.EarnedValue.Should().Be(0);
        indicators.PlannedValue.Should().Be(0);
        indicators.EstimateAtCompletion.Should().BeNull();
        indicators.VarianceAtCompletion.Should().BeNull();
    }
}
