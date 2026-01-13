using agileTrackerServer.Models.Entities;

namespace agileTrackerServer.Repositories.Interfaces;

public interface IUserStoryRepository
{
    Task<UserStory?> GetByIdAsync(int storyId);
    Task AddAsync(UserStory story);
    Task<UserStory?> GetByIdWithEpicAsync(int storyId);
    Task<List<UserStory>> GetByEpicIdAsync(int epicId);
    Task<int> GetNextPositionAsync(int epicId);
    Task ShiftPositionsAsync(int epicId, int fromPosition, int toPosition);
    Task<int> GetMaxPositionAsync(int epicId);
    Task SetPositionAsync(int storyId, int position);
    Task SetEpicAndPositionAsync(int storyId, int epicId, int position);
    Task DecrementPositionsAfterAsync(int epicId, int position);
    Task IncrementPositionsFromAsync(int epicId, int position);
    Task SaveChangesAsync();
}