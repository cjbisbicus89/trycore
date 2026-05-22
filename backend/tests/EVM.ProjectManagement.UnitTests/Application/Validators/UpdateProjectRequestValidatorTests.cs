namespace EVM.ProjectManagement.UnitTests.Application.Validators;

using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Application.Projects.Validators;
using Xunit;

public sealed class UpdateProjectRequestValidatorTests
{
    private readonly UpdateProjectRequestValidator validator = new();

    [Fact]
    public void ValidateReturnsErrorWhenNameIsEmpty()
    {
        var request = new UpdateProjectRequest(string.Empty, "Description");
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsSuccessWhenValid()
    {
        var request = new UpdateProjectRequest("Name", "Description");
        var result = this.validator.Validate(request);
        Assert.True(result.IsValid);
    }
}
