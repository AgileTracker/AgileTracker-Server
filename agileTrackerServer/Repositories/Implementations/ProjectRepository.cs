using agileTrackerServer.Data;
using agileTrackerServer.Models.Entities;
using agileTrackerServer.Models.Enums;
using agileTrackerServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace agileTrackerServer.Repositories.Implementations
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Project>> GetAllAsync(Guid ownerId) =>
            await _context.Projects.Include(p => p.Owner)
                .Where(p  => 
                    p.OwnerId == ownerId && 
                    p.Status == ProjectStatus.Active)
                .ToListAsync();

        public async Task<Project?> GetByIdAsync(Guid id, Guid ownerId) =>
            await _context.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.OwnerId == ownerId &&
                    p.Status == ProjectStatus.Active
                );
        
        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}