using agileTrackerServer.Data;
using agileTrackerServer.Models.Entities;
using agileTrackerServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace agileTrackerServer.Repositories.Implementations;

public class UserStoryRepository : IUserStoryRepository
{
    private readonly AppDbContext _context;

    public UserStoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserStory?> GetByIdAsync(int storyId)
        => await _context.UserStories.FirstOrDefaultAsync(us => us.Id == storyId);

    public async Task AddAsync(UserStory story)
        => await _context.UserStories.AddAsync(story);
    
    public async Task<UserStory?> GetByIdWithEpicAsync(int storyId)
    {
        return await _context.UserStories
            .Include(s => s.Epic)
            .FirstOrDefaultAsync(s => s.Id == storyId);
    }
    
    public async Task<List<UserStory>> GetByEpicIdAsync(int epicId)
    {
        return await _context.UserStories
            .Where(s => s.EpicId == epicId)
            .OrderBy(s => s.Position)
            .ToListAsync();
    }
    
    public async Task<int> GetNextPositionAsync(int epicId)
    {
        var max = await _context.UserStories
            .Where(s => s.EpicId == epicId)
            .Select(s => (int?)s.Position)
            .MaxAsync();

        return (max ?? -1) + 1;
    }


    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}