namespace EVM.ProjectManagement.UnitTests.Application;

using EVM.ProjectManagement.Application.Activities;
using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.Domain.ValueObjects;
using EVM.ProjectManagement.UnitTests.Builders;
using FluentAssertions;
using Moq;
using Xunit;

public sealed class ActivityServiceTests
{
    private readonly Mock<IActivityRepository> _activityRepositoryMock = new();
    private readonly Mock<IProjectRepository> _projectRepositoryMock = new();
    private readonly Mock<IEVMCalculator> _evmCalculatorMock = new();
    private readonly ActivityService _activityService;

    public ActivityServiceTests()
    {
        _activityService = new ActivityService(
            _activityRepositoryMock.Object,
            _projectRepositoryMock.Object,
            _evmCalculatorMock.Object,
            Mock.Of<Microsoft.Extensions.Logging.ILogger<ActivityService>>());
    }

    [Fact]
    public async Task CreateAsync_WhenProjectDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var request = new CreateActivityRequest(projectId, "Test Activity", 1000, 50, 25, 300);
        _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act & Assert
        var action = async () => await _activityService.CreateAsync(request);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetByIdAsync_WhenActivityDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        _activityRepositoryMock.Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Activity?)null);

        // Act & Assert
        var action = async () => await _activityService.GetByIdAsync(activityId);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetByProjectIdAsync_WhenCalled_ReturnsActivities()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var activities = new List<Activity> { new ActivityBuilder().WithProjectId(projectId).Build() };
        _activityRepositoryMock.Setup(x => x.GetByProjectIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activities);
        _evmCalculatorMock.Setup(x => x.Calculate(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Returns(EVMIndicators.Empty);

        // Act
        var result = await _activityService.GetByProjectIdAsync(projectId);

        // Assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreateAsync_WhenProjectExists_ReturnsActivity()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var request = new CreateActivityRequest(projectId, "Test Activity", 1000, 50, 25, 300);
        var project = new ProjectBuilder().Build();
        _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        _evmCalculatorMock.Setup(x => x.Calculate(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Returns(EVMIndicators.Empty);

        // Act
        var result = await _activityService.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Activity");
    }

    [Fact]
    public async Task UpdateAsync_WhenActivityDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var request = new UpdateActivityRequest("Updated", 1000, 50, 25, 300);
        _activityRepositoryMock.Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Activity?)null);

        // Act & Assert
        var action = async () => await _activityService.UpdateAsync(activityId, request);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenActivityDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        _activityRepositoryMock.Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Activity?)null);

        // Act & Assert
        var action = async () => await _activityService.DeleteAsync(activityId);
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
