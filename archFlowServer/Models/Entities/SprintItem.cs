using archFlowServer.Models.Exceptions;

namespace archFlowServer.Models.Entities;

public class SprintItem
{
    public int Id { get; private set; }
    public Guid SprintId { get; private set; }
    public int UserStoryId { get; private set; }

    public int Position { get; private set; } 
    public string Notes { get; private set; } = string.Empty;

    public DateTime AddedAt { get; private set; }

    public Sprint Sprint { get; private set; } = null!;
    public UserStory UserStory { get; private set; } = null!;

    private SprintItem() { } // EF

    internal SprintItem(Guid sprintId, int userStoryId, int position, string? notes)
    {
        if (sprintId == Guid.Empty)
            throw new DomainException("SprintId inválido.");

        if (userStoryId <= 0)
            throw new DomainException("UserStoryId inválido.");

        if (position < 0)
            throw new ValidationException("Position inválida.");

        SprintId = sprintId;
        UserStoryId = userStoryId;

        Position = position;
        Notes = notes?.Trim() ?? string.Empty;

        AddedAt = DateTime.UtcNow;
    }

    public void UpdatePlan(int position, string? notes)
    {
        if (position < 0)
            throw new ValidationException("Position inválida.");

        Position = position;
        Notes = notes?.Trim() ?? string.Empty;
    }
}
