namespace archFlowServer.Models.Dtos.Sprint;

public sealed record SprintResponseDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    string Goal,
    string ExecutionPlan,
    DateTime StartDate,
    DateTime EndDate,
    SprintStatusDto Status,
    int CapacityHours,
    bool IsArchived,
    DateTime? ArchivedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);