namespace EVM.ProjectManagement.UnitTests.Domain;

using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Exceptions;
using EVM.ProjectManagement.UnitTests.Builders;
using Xunit;

public sealed class ActivityInvariantTests
{
    [Fact]
    public void CreateLanzaDomainExceptionCuandoBudgetedCostMenorOIgualACero()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() => Activity.Create(projectId, "Test", 0, 50, 25, 300));
        Assert.Throws<DomainException>(() => Activity.Create(projectId, "Test", -100, 50, 25, 300));
    }

    [Fact]
    public void CreateLanzaDomainExceptionCuandoActualPercentageMayorACien()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() => Activity.Create(projectId, "Test", 1000, 50, 101, 300));
        Assert.Throws<DomainException>(() => Activity.Create(projectId, "Test", 1000, 50, 150, 300));
    }

    [Fact]
    public void CreateLanzaDomainExceptionCuandoPlannedPercentageMenorACero()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() => Activity.Create(projectId, "Test", 1000, -1, 25, 300));
        Assert.Throws<DomainException>(() => Activity.Create(projectId, "Test", 1000, -50, 25, 300));
    }

    [Fact]
    public void CreateLanzaDomainExceptionCuandoActualCostMenorACero()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() => Activity.Create(projectId, "Test", 1000, 50, 25, -1));
        Assert.Throws<DomainException>(() => Activity.Create(projectId, "Test", 1000, 50, 25, -100));
    }

    [Fact]
    public void CreateLanzaDomainExceptionCuandoNameVacio()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() => Activity.Create(projectId, string.Empty, 1000, 50, 25, 300));
        Assert.Throws<DomainException>(() => Activity.Create(projectId, null!, 1000, 50, 25, 300));
    }

    [Fact]
    public void EarnedValueCalculadoCorrectamente()
    {
        // Arrange
        var activity = new ActivityBuilder()
            .WithBudget(1000)
            .WithActualPercentage(50)
            .Build();

        // Act
        var earnedValue = activity.EarnedValue;

        // Assert
        Assert.Equal(500, earnedValue); // 1000 * (50 / 100) = 500
    }

    [Fact]
    public void PlannedValueCalculadoCorrectamente()
    {
        // Arrange
        var activity = new ActivityBuilder()
            .WithBudget(1000)
            .WithPlannedPercentage(60)
            .Build();

        // Act
        var plannedValue = activity.PlannedValue;

        // Assert
        Assert.Equal(600, plannedValue); // 1000 * (60 / 100) = 600
    }
}
