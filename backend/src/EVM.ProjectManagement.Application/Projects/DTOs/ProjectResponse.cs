namespace EVM.ProjectManagement.Application.Projects.DTOs;

using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Domain.ValueObjects;

public sealed record ProjectResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    EVMIndicators? Indicators,
    IReadOnlyList<ActivityResponse> Activities);
