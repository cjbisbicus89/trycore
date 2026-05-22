namespace EVM.ProjectManagement.UnitTests.Application.Validators;

using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Application.Projects.Validators;
using Xunit;

public sealed class CreateProjectRequestValidatorTests
{
    private readonly CreateProjectRequestValidator validator = new();

    [Fact]
    public void ValidateReturnsErrorWhenNameIsEmpty()
    {
        var request = new CreateProjectRequest(string.Empty, "Description");
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsErrorWhenDescriptionIsEmpty()
    {
        var request = new CreateProjectRequest("Name", string.Empty);
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsSuccessWhenValid()
    {
        var request = new CreateProjectRequest("Name", "Description");
        var result = this.validator.Validate(request);
        Assert.True(result.IsValid);
    }
}
