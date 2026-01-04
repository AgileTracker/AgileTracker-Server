using agileTrackerServer.Models.Entities;
using agileTrackerServer.Models.Enums;
using agileTrackerServer.Models.Exceptions;
using agileTrackerServer.Repositories.Interfaces;

namespace agileTrackerServer.Services;

public class ProjectAuthorizationService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectAuthorizationService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Project> AuthorizeAsync(
        Guid projectId,
        Guid userId,
        params MemberRole[] allowedRoles)
    {
        var project = await _projectRepository.GetByIdAsync(projectId, userId)
                      ?? throw new DomainException("Projeto não encontrado.");

        if (!project.HasPermission(userId, allowedRoles))
            throw new DomainException("Permissão insuficiente para acessar este recurso.");

        return project;
    }
}