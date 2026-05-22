namespace EVM.ProjectManagement.UnitTests.Application.Mappers;

using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Application.Projects.Extensions;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.Domain.ValueObjects;
using EVM.ProjectManagement.UnitTests.Builders;
using Xunit;

public sealed class ProjectMappingExtensionsTests
{
    [Fact]
    public void ToResponseMapsProjectWithIndicatorsAndActivities()
    {
        // Arrange
        var project = new ProjectBuilder().Build();
        var indicators = new EVMIndicators(0, 0, 0, 0, 0, null, null, null, null, EVMStatus.CostoNoAplicable, EVMStatus.CronogramaNoAplicable);
        var activities = new List<ActivityResponse>();

        // Act
        var response = project.ToResponse(indicators, activities);

        // Assert
        Assert.Equal(project.Id, response.Id);
        Assert.Equal(project.Name, response.Name);
        Assert.Equal(indicators, response.Indicators);
        Assert.Equal(activities, response.Activities);
    }

    [Fact]
    public void ToResponseMapsActivityWithIndicators()
    {
        // Arrange
        var activity = new ActivityBuilder().Build();
        var indicators = new EVMIndicators(0, 0, 0, 0, 0, null, null, null, null, EVMStatus.CostoNoAplicable, EVMStatus.CronogramaNoAplicable);

        // Act
        var response = activity.ToResponse(indicators);

        // Assert
        Assert.Equal(activity.Id, response.Id);
        Assert.Equal(activity.ProjectId, response.ProjectId);
        Assert.Equal(activity.Name, response.Name);
        Assert.Equal(indicators, response.Indicators);
    }
}
