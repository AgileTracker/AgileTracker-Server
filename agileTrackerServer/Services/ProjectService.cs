using agileTrackerServer.Models.Dtos.Project;
using agileTrackerServer.Models.Entities;
using agileTrackerServer.Models.Enums;
using agileTrackerServer.Models.Exceptions;
using agileTrackerServer.Repositories.Interfaces;

namespace agileTrackerServer.Services;

public class ProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync(Guid userId)
    {
        var projects = await _repository.GetAllAsync(userId);
        return projects.Select(MapToDto);
    }

    public async Task<ProjectResponseDto> GetByIdAsync(Guid projectId, Guid userId)
    {
        var project = await _repository.GetByIdAsync(projectId, userId)
            ?? throw new DomainException("Projeto não encontrado.");

        return MapToDto(project);
    }

    public async Task<ProjectResponseDto> CreateAsync(
        CreateProjectDto dto,
        Guid creatorUserId)
    {
        var project = new Project(
            dto.Name,
            dto.Description,
            creatorUserId
        );

        await _repository.AddAsync(project);
        await _repository.SaveChangesAsync();

        return MapToDto(project);
    }


    public async Task<ProjectResponseDto> UpdateAsync(
        Guid projectId,
        UpdateProjectDto dto,
        Guid executorUserId)
    {
        var project = await _repository.GetByIdAsync(projectId, executorUserId)
            ?? throw new DomainException("Projeto não encontrado.");

        project.UpdateDetails(
            executorUserId,
            dto.Name,
            dto.Description
        );

        await _repository.SaveChangesAsync();

        return MapToDto(project);
    }

    public async Task ArchiveAsync(Guid projectId, Guid executorUserId)
    {
        var project = await _repository.GetByIdAsync(projectId, executorUserId)
            ?? throw new DomainException("Projeto não encontrado.");

        project.Archive(executorUserId);

        await _repository.SaveChangesAsync();
    }
    
    public async Task AddMemberAsync(
        Guid projectId,
        Guid executorUserId,
        Guid newUserId,
        MemberRole role)
    {
        var project = await _repository.GetByIdAsync(projectId, executorUserId)
                      ?? throw new DomainException("Projeto não encontrado.");

        project.AddMember(executorUserId, newUserId, role);

        await _repository.SaveChangesAsync();
    }

    public async Task RemoveMemberAsync(
        Guid projectId,
        Guid executorUserId,
        Guid userId)
    {
        var project = await _repository.GetByIdAsync(projectId, executorUserId)
                      ?? throw new DomainException("Projeto não encontrado.");

        project.RemoveMember(executorUserId, userId);

        await _repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProjectMemberResponseDto>> GetMembersAsync(
        Guid projectId,
        Guid executorUserId)
    {
        // 1. Valida existência + acesso ao projeto
        var project = await _repository.GetByIdAsync(projectId, executorUserId)
                      ?? throw new DomainException("Projeto não encontrado.");

        // 2. Busca membros
        var members = await _repository.GetMembersAsync(
            projectId,
            executorUserId
        );

        // 3. Mapeia para DTO
        return members.Select(m => new ProjectMemberResponseDto
        {
            UserId = m.UserId,
            Name = m.User.Name,
            Email = m.User.Email,
            Role = m.Role,
            JoinedAt = m.JoinedAt
        });
    }

    
    private static ProjectResponseDto MapToDto(Project project)
    {
        return new ProjectResponseDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Status = project.Status,
            CreatedAt = project.CreatedAt
        };
    }
}
