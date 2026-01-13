using agileTrackerServer.Data;
using agileTrackerServer.Models.Entities;
using agileTrackerServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace agileTrackerServer.Repositories.Implementations;

public class EpicRepository : IEpicRepository
{
    private readonly AppDbContext _context;

    public EpicRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Epic?> GetByIdAsync(int epicId)
        => await _context.Epics.FirstOrDefaultAsync(e => e.Id == epicId);

    public async Task AddAsync(Epic epic)
        => await _context.Epics.AddAsync(epic);
    
    public async Task<List<Epic>> GetByBacklogIdAsync(Guid productBacklogId)
    {
        return await _context.Epics
            .Where(e => e.ProductBacklogId == productBacklogId)
            .OrderBy(e => e.Position)
            .ToListAsync();
    }
    
    public async Task<int> GetNextPositionAsync(Guid productBacklogId)
    {
        var max = await _context.Epics
            .Where(e => e.ProductBacklogId == productBacklogId)
            .Select(e => (int?)e.Position)
            .MaxAsync();

        return (max ?? -1) + 1;
    }

    public async Task ShiftPositionsAsync(Guid productBacklogId, int fromPosition, int toPosition)
    {
        if (fromPosition == toPosition) return;

        // Move para cima (ex.: 7 -> 2): itens [2..6] descem +1
        if (toPosition < fromPosition)
        {
            await _context.Epics
                .Where(e => e.ProductBacklogId == productBacklogId
                            && e.Position >= toPosition
                            && e.Position < fromPosition)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(e => e.Position, e => e.Position + 1));
            return;
        }

        // Move para baixo (ex.: 2 -> 7): itens [3..7] sobem -1
        await _context.Epics
            .Where(e => e.ProductBacklogId == productBacklogId
                        && e.Position > fromPosition
                        && e.Position <= toPosition)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(e => e.Position, e => e.Position - 1));
    }
    
    public async Task<int> GetMaxPositionAsync(Guid productBacklogId)
    {
        var max = await _context.Epics
            .Where(e => e.ProductBacklogId == productBacklogId)
            .Select(e => (int?)e.Position)
            .MaxAsync();

        return max ?? -1;
    }

    public async Task SetPositionAsync(int epicId, int position)
    {
        await _context.Epics
            .Where(e => e.Id == epicId)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.Position, position));
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}