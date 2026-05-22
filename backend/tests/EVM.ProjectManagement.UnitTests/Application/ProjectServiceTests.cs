namespace EVM.ProjectManagement.UnitTests.Application;

using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Application.Projects;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.Domain.ValueObjects;
using EVM.ProjectManagement.UnitTests.Builders;
using FluentAssertions;
using Moq;
using Xunit;

public sealed class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock = new();
    private readonly Mock<IEVMCalculator> _evmCalculatorMock = new();
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        _projectService = new ProjectService(_projectRepositoryMock.Object, _evmCalculatorMock.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<ProjectService>>());
    }

    [Fact]
    public async Task GetByIdAsync_WhenProjectDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _projectRepositoryMock.Setup(x => x.GetWithActivitiesAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act & Assert
        var action = async () => await _projectService.GetByIdAsync(projectId);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_WhenCalled_CallsAddAsync()
    {
        // Arrange
        var request = new CreateProjectRequest("Test Project", "Test Description");
        _projectRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _projectService.CreateAsync(request);

        // Assert
        _projectRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenProjectDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act & Assert
        var action = async () => await _projectService.DeleteAsync(projectId);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetByIdAsync_WhenProjectHasNoActivities_ReturnsEVMIndicatorsEmpty()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new ProjectBuilder().Build();
        _projectRepositoryMock.Setup(x => x.GetWithActivitiesAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        _evmCalculatorMock.Setup(x => x.Calculate(0, 0, 0, 0))
            .Returns(EVMIndicators.Empty);

        // Act
        var result = await _projectService.GetByIdAsync(projectId);

        // Assert
        result.Should().NotBeNull();
        result.Activities.Should().BeEmpty();
        result.Indicators.Should().Be(EVMIndicators.Empty);
        result.Indicators.PlannedValue.Should().Be(0);
        result.Indicators.EarnedValue.Should().Be(0);
        result.Indicators.ActualCost.Should().Be(0);
        result.Indicators.CostPerformanceIndex.Should().BeNull();
        result.Indicators.SchedulePerformanceIndex.Should().BeNull();
        result.Indicators.EstimateAtCompletion.Should().BeNull();
        result.Indicators.VarianceAtCompletion.Should().BeNull();
        result.Indicators.CostStatus.Should().Be(EVMStatus.CostoNoAplicable);
        result.Indicators.ScheduleStatus.Should().Be(EVMStatus.CronogramaNoAplicable);
    }

    [Fact]
    public async Task UpdateAsync_WhenProjectDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var request = new UpdateProjectRequest("Updated Name", "Updated Description");
        _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act & Assert
        var action = async () => await _projectService.UpdateAsync(projectId, request);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetAllAsync_WhenCalled_ReturnsProjectList()
    {
        // Arrange
        var projects = new List<Project> { new ProjectBuilder().Build(), new ProjectBuilder().WithName("P2").Build() };
        _projectRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);

        // Act
        var result = await _projectService.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(0, 0, 500, 0)]
    [InlineData(0, 0, 1000, 0)]
    public async Task GetByIdAsync_WhenBACIsZero_ReturnsZeroForEvAndPvButNullForEacAndVac(
        decimal plannedValue,
        decimal earnedValue,
        decimal actualCost,
        decimal budgetedCost)
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new ProjectBuilder().Build();
        _projectRepositoryMock.Setup(x => x.GetWithActivitiesAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        _evmCalculatorMock.Setup(x => x.Calculate(plannedValue, earnedValue, actualCost, budgetedCost))
            .Returns(new EVMIndicators(
                plannedValue,
                earnedValue,
                actualCost,
                earnedValue - actualCost,
                earnedValue - plannedValue,
                null,
                null,
                null,
                null,
                EVMStatus.CostoNoAplicable,
                EVMStatus.CronogramaNoAplicable));

        // Act
        var result = await _projectService.GetByIdAsync(projectId);

        // Assert
        result.Indicators!.EarnedValue.Should().Be(0);
        result.Indicators!.PlannedValue.Should().Be(0);
        result.Indicators!.EstimateAtCompletion.Should().BeNull();
        result.Indicators!.VarianceAtCompletion.Should().BeNull();
    }
}
