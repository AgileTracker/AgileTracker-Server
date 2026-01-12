using agileTrackerServer.Models.Entities;

namespace agileTrackerServer.Repositories.Interfaces;

public interface IUserStoryRepository
{
    Task<UserStory?> GetByIdAsync(int storyId);
    Task AddAsync(UserStory story);
    Task<UserStory?> GetByIdWithEpicAsync(int storyId);
    Task<List<UserStory>> GetByEpicIdAsync(int epicId);
    Task<int> GetNextPositionAsync(int epicId);

    Task SaveChangesAsync();
}