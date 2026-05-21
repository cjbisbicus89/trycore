using EVM.ProjectManagement.Domain.ValueObjects;

namespace EVM.ProjectManagement.Application.Projects.DTOs;

public sealed record ProjectResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    EVMIndicators? Indicators,
    IReadOnlyList<ActivityResponse> Activities);
