using archFlowServer.Models.Enums;
using archFlowServer.Models.Exceptions;

namespace archFlowServer.Models.Entities;

public class Board
{
    public Guid Id { get; private set; }

    public Guid ProjectId { get; private set; }
    public Guid SprintId { get; private set; } // <-- NOT NULL (1 board por sprint)

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public BoardType BoardType { get; private set; } = BoardType.Kanban;

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // navigations
    public Project Project { get; private set; } = null!;
    public Sprint Sprint { get; private set; } = null!;

    private Board() { } // EF

    internal Board(
        Guid projectId,
        Guid sprintId,
        string name,
        string? description,
        BoardType boardType)
    {
        if (projectId == Guid.Empty)
            throw new DomainException("ProjectId inválido.");

        if (sprintId == Guid.Empty)
            throw new DomainException("SprintId inválido.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Nome do board é obrigatório.");

        Id = Guid.NewGuid();
        ProjectId = projectId;
        SprintId = sprintId;

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        BoardType = boardType;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description, BoardType boardType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Nome do board é obrigatório.");

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        BoardType = boardType;

        UpdatedAt = DateTime.UtcNow;
    }
}
