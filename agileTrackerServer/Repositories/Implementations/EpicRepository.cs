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


    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}