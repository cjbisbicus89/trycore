namespace EVM.ProjectManagement.UnitTests.Application.Validators;

using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Activities.Validators;
using Xunit;

public sealed class CreateActivityRequestValidatorTests
{
    private readonly CreateActivityRequestValidator validator = new();

    [Fact]
    public void ValidateReturnsErrorWhenProjectIdIsEmpty()
    {
        var request = new CreateActivityRequest(Guid.Empty, "Name", 1000, 50, 25, 300);
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsErrorWhenNameIsEmpty()
    {
        var request = new CreateActivityRequest(Guid.NewGuid(), string.Empty, 1000, 50, 25, 300);
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsErrorWhenBudgetedCostIsZero()
    {
        var request = new CreateActivityRequest(Guid.NewGuid(), "Name", 0, 50, 25, 300);
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsErrorWhenPlannedPercentageOutOfRange()
    {
        var request = new CreateActivityRequest(Guid.NewGuid(), "Name", 1000, 101, 25, 300);
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsErrorWhenActualPercentageOutOfRange()
    {
        var request = new CreateActivityRequest(Guid.NewGuid(), "Name", 1000, 50, -1, 300);
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsErrorWhenActualCostIsNegative()
    {
        var request = new CreateActivityRequest(Guid.NewGuid(), "Name", 1000, 50, 25, -1);
        var result = this.validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateReturnsSuccessWhenValid()
    {
        var request = new CreateActivityRequest(Guid.NewGuid(), "Name", 1000, 50, 25, 300);
        var result = this.validator.Validate(request);
        Assert.True(result.IsValid);
    }
}
