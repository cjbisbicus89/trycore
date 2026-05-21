using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Application.Projects;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.UnitTests.Builders;
using Moq;
using Xunit;

namespace EVM.ProjectManagement.UnitTests.Application;

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
    public async Task GetByIdAsync_LanzaNotFoundException_CuandoProyectoNoExiste()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _projectRepositoryMock.Setup(x => x.GetWithActivitiesAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _projectService.GetByIdAsync(projectId));
    }

    [Fact]
    public async Task CreateAsync_LlamaAAddAsync()
    {
        // Arrange
        var request = new CreateProjectRequest("Test Project", "Test Description");
        _projectRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Project>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _projectService.CreateAsync(request);

        // Assert
        _projectRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Project>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_LanzaNotFoundException_CuandoProyectoNoExiste()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _projectService.DeleteAsync(projectId));
    }
}
