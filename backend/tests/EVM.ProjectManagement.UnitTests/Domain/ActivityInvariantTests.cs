namespace EVM.ProjectManagement.UnitTests.Domain;

using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Exceptions;
using EVM.ProjectManagement.UnitTests.Builders;
using FluentAssertions;
using Xunit;

public sealed class ActivityInvariantTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Create_WhenBudgetedCostIsLessThanOrEqualToZero_ThrowsDomainException(decimal budgetedCost)
    {
        // Arrange
        var activityBuilder = new ActivityBuilder().WithBudget(budgetedCost);

        // Act & Assert
        var action = () => activityBuilder.Build();
        action.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(101)]
    [InlineData(150)]
    [InlineData(200)]
    public void Create_WhenActualPercentageIsGreaterThan100_ThrowsDomainException(decimal actualPercentage)
    {
        // Arrange
        var activityBuilder = new ActivityBuilder().WithActualPercentage(actualPercentage);

        // Act & Assert
        var action = () => activityBuilder.Build();
        action.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-50)]
    [InlineData(-100)]
    public void Create_WhenPlannedPercentageIsLessThanZero_ThrowsDomainException(decimal plannedPercentage)
    {
        // Arrange
        var activityBuilder = new ActivityBuilder().WithPlannedPercentage(plannedPercentage);

        // Act & Assert
        var action = () => activityBuilder.Build();
        action.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-500)]
    public void Create_WhenActualCostIsLessThanZero_ThrowsDomainException(decimal actualCost)
    {
        // Arrange
        var activityBuilder = new ActivityBuilder().WithActualCost(actualCost);

        // Act & Assert
        var action = () => activityBuilder.Build();
        action.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WhenNameIsNullOrWhiteSpace_ThrowsDomainException(string name)
    {
        // Arrange
        var activityBuilder = new ActivityBuilder().WithName(name);

        // Act & Assert
        var action = () => activityBuilder.Build();
        action.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(1000, 50, 500)]
    [InlineData(2000, 25, 500)]
    [InlineData(5000, 75, 3750)]
    public void EarnedValue_WhenCalculated_ReturnsCorrectValue(
        decimal budgetedCost,
        decimal actualPercentage,
        decimal expectedEarnedValue)
    {
        // Arrange
        var activity = new ActivityBuilder()
            .WithBudget(budgetedCost)
            .WithActualPercentage(actualPercentage)
            .Build();

        // Act
        var earnedValue = activity.EarnedValue;

        // Assert
        earnedValue.Should().Be(expectedEarnedValue);
    }

    [Theory]
    [InlineData(1000, 60, 600)]
    [InlineData(2000, 40, 800)]
    [InlineData(5000, 80, 4000)]
    public void PlannedValue_WhenCalculated_ReturnsCorrectValue(
        decimal budgetedCost,
        decimal plannedPercentage,
        decimal expectedPlannedValue)
    {
        // Arrange
        var activity = new ActivityBuilder()
            .WithBudget(budgetedCost)
            .WithPlannedPercentage(plannedPercentage)
            .Build();

        // Act
        var plannedValue = activity.PlannedValue;

        // Assert
        plannedValue.Should().Be(expectedPlannedValue);
    }

    [Theory]
    [InlineData(1000, 0)]
    [InlineData(2000, 0)]
    [InlineData(5000, 0)]
    public void EarnedValue_WhenActualProgressIsZero_ReturnsZero(
        decimal budgetedCost,
        decimal expectedEarnedValue)
    {
        // Arrange
        var activity = new ActivityBuilder()
            .WithBudget(budgetedCost)
            .WithActualPercentage(0)
            .Build();

        // Act
        var earnedValue = activity.EarnedValue;

        // Assert
        earnedValue.Should().Be(expectedEarnedValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Update_WhenBudgetedCostIsLessThanOrEqualToZero_ThrowsDomainException(decimal budgetedCost)
    {
        // Arrange
        var activity = new ActivityBuilder().Build();

        // Act & Assert
        var action = () => activity.Update("Test", budgetedCost, 50, 25, 300);
        action.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(150)]
    public void Update_WhenPlannedPercentageIsOutOfRange_ThrowsDomainException(decimal plannedPercentage)
    {
        // Arrange
        var activity = new ActivityBuilder().Build();

        // Act & Assert
        var action = () => activity.Update("Test", 1000, plannedPercentage, 25, 300);
        action.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(150)]
    public void Update_WhenActualPercentageIsOutOfRange_ThrowsDomainException(decimal actualPercentage)
    {
        // Arrange
        var activity = new ActivityBuilder().Build();

        // Act & Assert
        var action = () => activity.Update("Test", 1000, 50, actualPercentage, 300);
        action.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-500)]
    public void Update_WhenActualCostIsLessThanZero_ThrowsDomainException(decimal actualCost)
    {
        // Arrange
        var activity = new ActivityBuilder().Build();

        // Act & Assert
        var action = () => activity.Update("Test", 1000, 50, 25, actualCost);
        action.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_WhenNameIsNullOrWhiteSpace_ThrowsDomainException(string name)
    {
        // Arrange
        var activity = new ActivityBuilder().Build();

        // Act & Assert
        var action = () => activity.Update(name, 1000, 50, 25, 300);
        action.Should().Throw<DomainException>();
    }
}
