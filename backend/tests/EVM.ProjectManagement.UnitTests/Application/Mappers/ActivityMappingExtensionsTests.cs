namespace EVM.ProjectManagement.UnitTests.Application.Mappers;

using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Activities.Extensions;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.Domain.ValueObjects;
using EVM.ProjectManagement.UnitTests.Builders;
using Moq;
using Xunit;

public sealed class ActivityMappingExtensionsTests
{
    [Fact]
    public void ToResponseMapsActivityWithIndicators()
    {
        // Arrange
        var activity = new ActivityBuilder().Build();
        var calculatorMock = new Mock<IEVMCalculator>();
        var indicators = new EVMIndicators(0, 0, 0, 0, 0, null, null, null, null, EVMStatus.CostoNoAplicable, EVMStatus.CronogramaNoAplicable);
        calculatorMock.Setup(x => x.Calculate(activity.PlannedValue, activity.EarnedValue, activity.ActualCost, activity.BudgetedCost))
            .Returns(indicators);

        // Act
        var response = activity.ToResponse(calculatorMock.Object);

        // Assert
        Assert.Equal(activity.Id, response.Id);
        Assert.Equal(activity.ProjectId, response.ProjectId);
        Assert.Equal(activity.Name, response.Name);
        Assert.Equal(indicators, response.Indicators);
    }
}
