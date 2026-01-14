using agileTrackerServer.Models.Enums;
using agileTrackerServer.Models.Exceptions;

namespace agileTrackerServer.Models.Entities;

public class UserStory
{
    public int Id { get; private set; }
    public int EpicId { get; private set; }

    public string Title { get; private set; } = string.Empty;
    public string Persona { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public string AcceptanceCriteria { get; private set; } = string.Empty;
    public UserStoryComplexity Complexity { get; private set; } = UserStoryComplexity.Medium;

    public int? Effort { get; private set; }
    public string Dependencies { get; private set; } = string.Empty;

    public int Priority { get; private set; } = 0;
    public BusinessValue BusinessValue { get; private set; } = BusinessValue.Medium;
    public UserStoryStatus Status { get; private set; } = UserStoryStatus.Draft;

    public int Position { get; private set; } // ✅ sem set público

    public Guid? AssigneeId { get; private set; }
    
    public bool IsArchived { get; private set; }
    public DateTime? ArchivedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Epic Epic { get; private set; } = null!;

    private UserStory() { } // EF

    internal UserStory(
        int epicId,
        string title,
        string persona,
        string description,
        string? acceptanceCriteria,
        UserStoryComplexity complexity,
        int? effort,
        string? dependencies,
        int priority,
        BusinessValue businessValue,
        UserStoryStatus status,
        int position,
        Guid? assigneeId,
        bool isArchived,
        DateTime? archivedAt)
    {
        if (epicId <= 0)
            throw new DomainException("EpicId inválido.");

        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException("Título é obrigatório.");

        if (string.IsNullOrWhiteSpace(persona))
            throw new ValidationException("Persona é obrigatória.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException("Descrição é obrigatória.");

        if (position < 0)
            throw new ValidationException("Position inválida.");

        EpicId = epicId;

        Title = title.Trim();
        Persona = persona.Trim();
        Description = description.Trim();

        AcceptanceCriteria = acceptanceCriteria?.Trim() ?? string.Empty;
        Complexity = complexity;

        Effort = effort;
        Dependencies = dependencies?.Trim() ?? string.Empty;

        Priority = priority;
        BusinessValue = businessValue;
        Status = status;

        Position = position;
        AssigneeId = assigneeId;
        
        IsArchived = isArchived;
        ArchivedAt = archivedAt;
        
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        string title,
        string persona,
        string description,
        string? acceptanceCriteria,
        UserStoryComplexity complexity,
        int? effort,
        string? dependencies,
        int priority,
        BusinessValue businessValue,
        UserStoryStatus status,
        int position,
        Guid? assigneeId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException("Título é obrigatório.");

        if (string.IsNullOrWhiteSpace(persona))
            throw new ValidationException("Persona é obrigatória.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException("Descrição é obrigatória.");

        if (position < 0)
            throw new ValidationException("Position inválida.");

        Title = title.Trim();
        Persona = persona.Trim();
        Description = description.Trim();

        AcceptanceCriteria = acceptanceCriteria?.Trim() ?? string.Empty;
        Complexity = complexity;

        Effort = effort;
        Dependencies = dependencies?.Trim() ?? string.Empty;

        Priority = priority;
        BusinessValue = businessValue;
        Status = status;

        Position = position;
        AssigneeId = assigneeId;

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
