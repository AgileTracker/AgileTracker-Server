using archFlowServer.Models.Enums;
using archFlowServer.Models.Exceptions;

namespace archFlowServer.Models.Entities;

public class Sprint
{
    private readonly List<SprintItem> _items = new();

    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string Goal { get; private set; } = string.Empty;
    public string ExecutionPlan { get; private set; } = string.Empty;

    // mantendo DateTime por compatibilidade com DTOs atuais
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public SprintStatus Status { get; private set; } = SprintStatus.Planned;
    public int CapacityHours { get; private set; } = 0;

    // soft-archive (alinhado com Epic/UserStory)
    public bool IsArchived { get; private set; }
    public DateTime? ArchivedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // navigations
    public Project Project { get; private set; } = null!;
    public Board Board { get; private set; } = null!;
    public IReadOnlyCollection<SprintItem> Items => _items.AsReadOnly();

    private Sprint() { } // EF

    internal Sprint(
        Guid projectId,
        string name,
        string? goal,
        string? executionPlan,
        DateTime startDate,
        DateTime endDate,
        int capacityHours)
    {
        if (projectId == Guid.Empty)
            throw new DomainException("ProjectId inválido.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Nome do sprint é obrigatório.");

        if (startDate.Date >= endDate.Date)
            throw new ValidationException("StartDate deve ser menor que EndDate.");

        if (capacityHours < 0)
            throw new ValidationException("CapacityHours não pode ser negativo.");

        Id = Guid.NewGuid();
        ProjectId = projectId;

        Name = name.Trim();
        Goal = goal?.Trim() ?? string.Empty;
        ExecutionPlan = executionPlan?.Trim() ?? string.Empty;

        StartDate = startDate.Date;
        EndDate = endDate.Date;

        Status = SprintStatus.Planned;
        CapacityHours = capacityHours;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string? goal, string? executionPlan, DateTime startDate, DateTime endDate, int capacityHours)
    {
        if (Status is SprintStatus.Closed or SprintStatus.Cancelled)
            throw new ConflictException("Não é possível editar sprint fechada/cancelada.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Nome do sprint é obrigatório.");

        if (startDate.Date >= endDate.Date)
            throw new ValidationException("StartDate deve ser menor que EndDate.");

        if (capacityHours < 0)
            throw new ValidationException("CapacityHours não pode ser negativo.");

        Name = name.Trim();
        Goal = goal?.Trim() ?? string.Empty;
        ExecutionPlan = executionPlan?.Trim() ?? string.Empty;
        StartDate = startDate.Date;
        EndDate = endDate.Date;
        CapacityHours = capacityHours;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status != SprintStatus.Planned)
            throw new ConflictException("Apenas sprints Planned podem ser ativadas.");

        Status = SprintStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Close()
    {
        if (Status != SprintStatus.Active)
            throw new ConflictException("Apenas sprints Active podem ser fechadas.");

        Status = SprintStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == SprintStatus.Closed)
            throw new ConflictException("Não é possível cancelar sprint já fechada.");

        Status = SprintStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Archive()
    {
        if (IsArchived) return;
        IsArchived = true;
        ArchivedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (!IsArchived) return;
        IsArchived = false;
        ArchivedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
