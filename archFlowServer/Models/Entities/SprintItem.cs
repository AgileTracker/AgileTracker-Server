using archFlowServer.Models.Exceptions;

namespace archFlowServer.Models.Entities;

public class SprintItem
{
    public int Id { get; private set; }           

    public Guid SprintId { get; private set; }
    public int UserStoryId { get; private set; }

    public DateTime AddedAt { get; private set; }

    // navigations
    public Sprint Sprint { get; private set; } = null!;
    public UserStory UserStory { get; private set; } = null!;

    private SprintItem() { } // EF

    internal SprintItem(Guid sprintId, int userStoryId)
    {
        if (sprintId == Guid.Empty)
            throw new DomainException("SprintId inválido.");

        if (userStoryId <= 0)
            throw new DomainException("UserStoryId inválido.");

        SprintId = sprintId;
        UserStoryId = userStoryId;
        AddedAt = DateTime.UtcNow;
    }
}
