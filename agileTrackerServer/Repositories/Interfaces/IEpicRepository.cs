using agileTrackerServer.Models.Entities;

namespace agileTrackerServer.Repositories.Interfaces;

public interface IEpicRepository
{
    Task<Epic?> GetByIdAsync(int epicId);
    Task AddAsync(Epic epic);
    Task<List<Epic>> GetByBacklogIdAsync(Guid productBacklogId);
    Task<int> GetNextPositionAsync(Guid productBacklogId);

    Task SaveChangesAsync();
}