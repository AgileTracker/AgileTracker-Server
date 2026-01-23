using archFlowServer.Data;
using archFlowServer.Models.Entities;
using archFlowServer.Models.Enums;
using archFlowServer.Models.Exceptions;
using archFlowServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace archFlowServer.Repositories.Implementations;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync(Guid userId)
    {
        return await _context.Projects
            .Include(p => p.Members)
            .Where(p =>
                p.Status == ProjectStatus.Active &&
                p.Members.Any(m => m.UserId == userId)
            )
            .ToListAsync();
    }

    public async Task<Project?> GetByIdIncludingArchivedAsync(Guid projectId, Guid userId)
    {
        return await _context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p =>
                p.Id == projectId &&
                p.Status == ProjectStatus.Archived &&
                p.Members.Any(m => m.UserId == userId)
            );
    }

    public async Task<Project?> GetByIdAsync(Guid projectId, Guid userId)
    {
        return await _context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p =>
                p.Id == projectId &&
                p.Status == ProjectStatus.Active &&
                p.Members.Any(m => m.UserId == userId)
            );
    }
    
    

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }
    
    public async Task<IEnumerable<ProjectMember>> GetMembersAsync(
        Guid projectId,
        Guid userId)
    {
        // garante que o usuário faz parte do projeto
        var hasAccess = await _context.ProjectMembers.AnyAsync(pm =>
            pm.ProjectId == projectId &&
            pm.UserId == userId);

        if (!hasAccess)
            throw new DomainException("Você não tem acesso a este projeto.");

        return await _context.ProjectMembers
            .Include(pm => pm.User)
            .Where(pm => pm.ProjectId == projectId)
            .OrderBy(pm => pm.JoinedAt)
            .ToListAsync();
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
