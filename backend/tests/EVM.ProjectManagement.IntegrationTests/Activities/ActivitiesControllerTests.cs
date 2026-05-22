namespace EVM.ProjectManagement.IntegrationTests.Activities;

using System.Net;
using System.Net.Http.Json;
using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public sealed class ActivitiesControllerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ActivitiesControllerTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetByProjectId_WhenProjectExists_ReturnsOkWithEmptyList()
    {
        // Act
        var response = await _client.GetAsync($"/api/activities/project/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var activities = await response.Content.ReadFromJsonAsync<IReadOnlyList<ActivityResponse>>();
        activities.Should().NotBeNull();
        activities.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_WhenActivityDoesNotExist_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/activities/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(404);
    }

    [Fact]
    public async Task Create_WhenProjectDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var request = new CreateActivityRequest(Guid.NewGuid(), "Test Activity", 1000, 50, 25, 300);

        // Act
        var response = await _client.PostAsJsonAsync("/api/activities", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(404);
    }

    [Fact]
    public async Task Create_WhenProjectExists_ReturnsCreatedWithActivityResponse()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest("Test Project", "Test Description");
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", createProjectRequest);
        var project = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var request = new CreateActivityRequest(project!.Id, "Test Activity", 1000, 50, 25, 300);

        // Act
        var response = await _client.PostAsJsonAsync("/api/activities", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var activity = await response.Content.ReadFromJsonAsync<ActivityResponse>();
        activity.Should().NotBeNull();
        activity!.Name.Should().Be("Test Activity");
        activity.BudgetedCost.Should().Be(1000);
        activity.ProjectId.Should().Be(project.Id);
    }

    [Fact]
    public async Task GetById_WhenActivityExists_ReturnsOkWithActivityResponse()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest("Test Project", "Test Description");
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", createProjectRequest);
        var project = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var createActivityRequest = new CreateActivityRequest(project!.Id, "Test Activity", 1000, 50, 25, 300);
        var activityResponse = await _client.PostAsJsonAsync("/api/activities", createActivityRequest);
        var activity = await activityResponse.Content.ReadFromJsonAsync<ActivityResponse>();

        // Act
        var response = await _client.GetAsync($"/api/activities/{activity!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedActivity = await response.Content.ReadFromJsonAsync<ActivityResponse>();
        retrievedActivity.Should().NotBeNull();
        retrievedActivity!.Id.Should().Be(activity.Id);
        retrievedActivity.Name.Should().Be("Test Activity");
    }

    [Fact]
    public async Task Update_WhenActivityExists_ReturnsOkWithUpdatedActivityResponse()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest("Test Project", "Test Description");
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", createProjectRequest);
        var project = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var createActivityRequest = new CreateActivityRequest(project!.Id, "Test Activity", 1000, 50, 25, 300);
        var activityResponse = await _client.PostAsJsonAsync("/api/activities", createActivityRequest);
        var activity = await activityResponse.Content.ReadFromJsonAsync<ActivityResponse>();
        var updateRequest = new UpdateActivityRequest("Updated Activity", 2000, 60, 30, 400);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/activities/{activity!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedActivity = await response.Content.ReadFromJsonAsync<ActivityResponse>();
        updatedActivity.Should().NotBeNull();
        updatedActivity!.Name.Should().Be("Updated Activity");
        updatedActivity.BudgetedCost.Should().Be(2000);
        updatedActivity.PlannedPercentage.Should().Be(60);
        updatedActivity.ActualPercentage.Should().Be(30);
        updatedActivity.ActualCost.Should().Be(400);
    }

    [Fact]
    public async Task Update_WhenActivityDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var updateRequest = new UpdateActivityRequest("Updated Activity", 2000, 60, 30, 400);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/activities/{Guid.NewGuid()}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(404);
    }

    [Fact]
    public async Task Delete_WhenActivityExists_ReturnsNoContent()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest("Test Project", "Test Description");
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", createProjectRequest);
        var project = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var createActivityRequest = new CreateActivityRequest(project!.Id, "Test Activity", 1000, 50, 25, 300);
        var activityResponse = await _client.PostAsJsonAsync("/api/activities", createActivityRequest);
        var activity = await activityResponse.Content.ReadFromJsonAsync<ActivityResponse>();

        // Act
        var response = await _client.DeleteAsync($"/api/activities/{activity!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_WhenActivityDoesNotExist_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/activities/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(404);
    }

    [Fact]
    public async Task Create_WhenRequestIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateActivityRequest(Guid.NewGuid(), "", -100, 150, -10, -500);

        // Act
        var response = await _client.PostAsJsonAsync("/api/activities", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
    }

    [Fact]
    public async Task Update_WhenRequestIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest("Test Project", "Test Description");
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", createProjectRequest);
        var project = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        var createActivityRequest = new CreateActivityRequest(project!.Id, "Test Activity", 1000, 50, 25, 300);
        var activityResponse = await _client.PostAsJsonAsync("/api/activities", createActivityRequest);
        var activity = await activityResponse.Content.ReadFromJsonAsync<ActivityResponse>();
        var updateRequest = new UpdateActivityRequest("", -100, 150, -10, -500);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/activities/{activity!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
    }
}
