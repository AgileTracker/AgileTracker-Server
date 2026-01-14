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
        => await _context.UserStories.FirstOrDefaultAsync(s => s.Id == storyId && !s.IsArchived);

    public async Task AddAsync(UserStory story)
        => await _context.UserStories.AddAsync(story);
    
    public async Task<UserStory?> GetByIdWithEpicAsync(int storyId)
    {
        return await _context.UserStories
            .Include(s => s.Epic)
            .FirstOrDefaultAsync(s => s.Id == storyId && !s.IsArchived);
    }
    
    public Task<UserStory?> GetByIdWithEpicIncludingArchivedAsync(int id)
        => _context.UserStories
            .Include(s => s.Epic)
            .FirstOrDefaultAsync(s => s.Id == id);
    public async Task<List<UserStory>> GetByEpicIdAsync(int epicId)
    {
        return await _context.UserStories
            .Where(s => s.EpicId == epicId && !s.IsArchived)
            .OrderBy(s => s.Position)
            .ToListAsync();
    }
    
    public async Task<int> GetNextPositionAsync(int epicId)
    {
        var max = await _context.UserStories
            .Where(s => s.EpicId == epicId && !s.IsArchived)
            .Select(s => (int?)s.Position)
            .MaxAsync();

        return (max ?? -1) + 1;
    }

    // ✅ usado para posição temporária
    public async Task<int> GetMaxPositionAsync(int epicId)
    {
        var max = await _context.UserStories
            .Where(s => s.EpicId == epicId && !s.IsArchived)
            .Select(s => (int?)s.Position)
            .MaxAsync();

        return max ?? -1;
    }

    // ✅ set direto no banco (sem tracking)
    public async Task SetPositionAsync(int storyId, int position)
    {
        await _context.UserStories
            .Where(s => s.Id == storyId && !s.IsArchived)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(s => s.Position, position));
    }

    // ✅ shift por faixa (drag-ready)
    public async Task ShiftPositionsAsync(int epicId, int fromPosition, int toPosition)
    {
        if (fromPosition == toPosition) return;

        // Move para cima: itens [to..from-1] descem +1
        if (toPosition < fromPosition)
        {
            await _context.UserStories
                .Where(s => s.EpicId == epicId
                            && s.Position >= toPosition
                            && s.Position < fromPosition
                            && !s.IsArchived)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(s => s.Position, s => s.Position + 1));
            return;
        }

        // Move para baixo: itens [from+1..to] sobem -1
        await _context.UserStories
            .Where(s => s.EpicId == epicId
                        && s.Position > fromPosition
                        && s.Position <= toPosition
                        && !s.IsArchived)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(s => s.Position, s => s.Position - 1));
    }
    
    public async Task SetEpicAndPositionAsync(int storyId, int epicId, int position)
    {
        await _context.UserStories
            .Where(s => s.Id == storyId && !s.IsArchived)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.EpicId, epicId)
                .SetProperty(s => s.Position, position));
    }

    // Fecha buraco no épico de origem: tudo que está depois da posição removida desce -1
    public async Task DecrementPositionsAfterAsync(int epicId, int position)
    {
        await _context.UserStories
            .Where(s => s.EpicId == epicId && s.Position > position && !s.IsArchived)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(s => s.Position, s => s.Position - 1));
    }

    // Abre espaço no épico de destino: tudo >= toPosition sobe +1
    public async Task IncrementPositionsFromAsync(int epicId, int position)
    {
        await _context.UserStories
            .Where(s => s.EpicId == epicId && s.Position >= position && !s.IsArchived)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(s => s.Position, s => s.Position + 1));
    }
    
    public async Task ArchiveAsync(int storyId)
    {
        await _context.UserStories
            .Where(s => s.Id == storyId && !s.IsArchived)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.IsArchived, true)
                .SetProperty(s => s.ArchivedAt, DateTime.UtcNow)
                .SetProperty(s => s.UpdatedAt, DateTime.UtcNow));
    }

    public async Task RestoreAsync(int storyId)
    {
        await _context.UserStories
            .Where(s => s.Id == storyId && s.IsArchived)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.IsArchived, false)
                .SetProperty(s => s.ArchivedAt, (DateTime?)null)
                .SetProperty(s => s.UpdatedAt, DateTime.UtcNow));
    }

    public async Task ArchiveByEpicIdAsync(int epicId)
    {
        await _context.UserStories
            .Where(s => s.EpicId == epicId && !s.IsArchived)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.IsArchived, true)
                .SetProperty(s => s.ArchivedAt, DateTime.UtcNow)
                .SetProperty(s => s.UpdatedAt, DateTime.UtcNow));
    }
    
    public async Task RestoreByEpicIdAsync(int epicId)
    {
        await _context.UserStories
            .Where(s => s.EpicId == epicId && s.IsArchived)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.IsArchived, false)
                .SetProperty(s => s.ArchivedAt, (DateTime?)null)
                .SetProperty(s => s.UpdatedAt, DateTime.UtcNow));
    }
    
    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}