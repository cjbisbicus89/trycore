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

    [Fact]
    public void EarnedValueEsCeroCuandoAvanceRealEsCero()
    {
        // Arrange
        var activity = new ActivityBuilder()
            .WithBudget(1000)
            .WithActualPercentage(0)
            .Build();

        // Act
        var earnedValue = activity.EarnedValue;

        // Assert
        Assert.Equal(0, earnedValue);
    }

    [Fact]
    public void UpdateLanzaDomainExceptionCuandoBudgetedCostMenorOIgualACero()
    {
        // Arrange
        var activity = new ActivityBuilder().Build();

        // Act & Assert
        Assert.Throws<DomainException>(() => activity.Update("Test", 0, 50, 25, 300));
        Assert.Throws<DomainException>(() => activity.Update("Test", -100, 50, 25, 300));
    }

    [Fact]
    public void UpdateLanzaDomainExceptionCuandoPlannedPercentageFueraDeRango()
    {
        // Arrange
        var activity = new ActivityBuilder().Build();

        // Act & Assert
        Assert.Throws<DomainException>(() => activity.Update("Test", 1000, -1, 25, 300));
        Assert.Throws<DomainException>(() => activity.Update("Test", 1000, 101, 25, 300));
    }

    [Fact]
    public void UpdateLanzaDomainExceptionCuandoActualPercentageFueraDeRango()
    {
        // Arrange
        var activity = new ActivityBuilder().Build();

        // Act & Assert
        Assert.Throws<DomainException>(() => activity.Update("Test", 1000, 50, -1, 300));
        Assert.Throws<DomainException>(() => activity.Update("Test", 1000, 50, 101, 300));
    }

    [Fact]
    public void UpdateLanzaDomainExceptionCuandoActualCostMenorACero()
    {
        // Arrange
        var activity = new ActivityBuilder().Build();

        // Act & Assert
        Assert.Throws<DomainException>(() => activity.Update("Test", 1000, 50, 25, -1));
        Assert.Throws<DomainException>(() => activity.Update("Test", 1000, 50, 25, -100));
    }

    [Fact]
    public void UpdateLanzaDomainExceptionCuandoNameVacio()
    {
        // Arrange
        var activity = new ActivityBuilder().Build();

        // Act & Assert
        Assert.Throws<DomainException>(() => activity.Update(string.Empty, 1000, 50, 25, 300));
        Assert.Throws<DomainException>(() => activity.Update(null!, 1000, 50, 25, 300));
    }
}
