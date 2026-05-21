namespace EVM.ProjectManagement.UnitTests.Application;

using EVM.ProjectManagement.Application.Activities;
using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.UnitTests.Builders;
using Moq;
using Xunit;

public sealed class ActivityServiceTests
{
    private readonly Mock<IActivityRepository> activityRepositoryMock = new();
    private readonly Mock<IProjectRepository> projectRepositoryMock = new();
    private readonly Mock<IEVMCalculator> evmCalculatorMock = new();
    private readonly ActivityService activityService;

    public ActivityServiceTests()
    {
        this.activityService = new ActivityService(
            this.activityRepositoryMock.Object,
            this.projectRepositoryMock.Object,
            this.evmCalculatorMock.Object,
            Mock.Of<Microsoft.Extensions.Logging.ILogger<ActivityService>>());
    }

    [Fact]
    public async Task CreateAsyncLanzaNotFoundExceptionCuandoProyectoNoExiste()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var request = new CreateActivityRequest(projectId, "Test Activity", 1000, 50, 25, 300);
        this.projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => this.activityService.CreateAsync(request));
    }

    [Fact]
    public async Task GetByIdAsyncLanzaNotFoundExceptionCuandoActividadNoExiste()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        this.activityRepositoryMock.Setup(x => x.GetByIdAsync(activityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Activity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => this.activityService.GetByIdAsync(activityId));
    }
}
