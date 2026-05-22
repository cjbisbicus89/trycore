namespace EVM.ProjectManagement.UnitTests.Application.Validators;

using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Activities.Validators;
using Xunit;

public sealed class UpdateActivityRequestValidatorTests
{
    private readonly UpdateActivityRequestValidator validator = new();

    [Fact]
    public void ValidateReturnsErrorWhenNameIsEmpty()
    {
        var request = new UpdateActivityRequest(string.Empty, 1000, 50, 25, 300);
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsErrorWhenBudgetedCostIsZero()
    {
        var request = new UpdateActivityRequest("Name", 0, 50, 25, 300);
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsSuccessWhenValid()
    {
        var request = new UpdateActivityRequest("Name", 1000, 50, 25, 300);
        var result = this.validator.Validate(request);
        Assert.True(result.IsValid);
    }
}
