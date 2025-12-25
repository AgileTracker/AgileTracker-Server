using agileTrackerServer.Models.Entities;

namespace agileTrackerServer.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync(Guid ownerId);
        Task<Project?> GetByIdAsync(Guid id, Guid OwnerId);
        Task AddAsync(Project project);
        Task SaveChangesAsync();
    }
}