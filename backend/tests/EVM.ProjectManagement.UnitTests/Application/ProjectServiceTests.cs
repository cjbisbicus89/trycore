namespace EVM.ProjectManagement.UnitTests.Application;

using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Application.Projects;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.UnitTests.Builders;
using Moq;
using Xunit;

public sealed class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> projectRepositoryMock = new();
    private readonly Mock<IEVMCalculator> evmCalculatorMock = new();
    private readonly ProjectService projectService;

    public ProjectServiceTests()
    {
        this.projectService = new ProjectService(this.projectRepositoryMock.Object, this.evmCalculatorMock.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<ProjectService>>());
    }

    [Fact]
    public async Task GetByIdAsyncLanzaNotFoundExceptionCuandoProyectoNoExiste()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        this.projectRepositoryMock.Setup(x => x.GetWithActivitiesAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => this.projectService.GetByIdAsync(projectId));
    }

    [Fact]
    public async Task CreateAsyncLlamaAAddAsync()
    {
        // Arrange
        var request = new CreateProjectRequest("Test Project", "Test Description");
        this.projectRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await this.projectService.CreateAsync(request);

        // Assert
        this.projectRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsyncLanzaNotFoundExceptionCuandoProyectoNoExiste()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        this.projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => this.projectService.DeleteAsync(projectId));
    }
}
