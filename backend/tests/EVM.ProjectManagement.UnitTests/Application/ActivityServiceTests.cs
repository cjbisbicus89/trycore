using EVM.ProjectManagement.Application.Activities;
using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.UnitTests.Builders;
using Moq;
using Xunit;

namespace EVM.ProjectManagement.UnitTests.Application;

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
    public async Task CreateAsync_LanzaNotFoundException_CuandoProyectoNoExiste()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var request = new CreateActivityRequest(projectId, "Test Activity", 1000, 50, 25, 300);
        _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Project?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _activityService.CreateAsync(request));
    }

    [Fact]
    public async Task GetByIdAsync_LanzaNotFoundException_CuandoActividadNoExiste()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        _activityRepositoryMock.Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Activity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _activityService.GetByIdAsync(activityId));
    }
}
