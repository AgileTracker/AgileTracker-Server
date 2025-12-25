using agileTrackerServer.Models.Dtos.Project;
using agileTrackerServer.Models.Entities;
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

    public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync(Guid ownerId)
    {
        var projects = await _repository.GetAllAsync(ownerId);

        return projects.Select(MapToDto);
    }

    public async Task<ProjectResponseDto> GetByIdAsync(Guid projectId, Guid ownerId)
    {
        var project = await _repository.GetByIdAsync(projectId, ownerId)
            ?? throw new DomainException("Projeto não encontrado.");

        return MapToDto(project);
    }

    public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto, Guid ownerId)
    {
        var project = new Project(dto.Name, dto.Description, ownerId);

        await _repository.AddAsync(project);
        await _repository.SaveChangesAsync();

        return MapToDto(project);
    }

    public async Task<ProjectResponseDto> UpdateAsync(
        Guid projectId,
        UpdateProjectDto dto,
        Guid ownerId)
    {
        var project = await _repository.GetByIdAsync(projectId, ownerId)
            ?? throw new DomainException("Projeto não encontrado.");

        project.UpdateDetails(dto.Name, dto.Description);

        await _repository.SaveChangesAsync();

        return MapToDto(project);
    }

    public async Task ArchiveAsync(Guid projectId, Guid ownerId)
    {
        var project = await _repository.GetByIdAsync(projectId, ownerId)
            ?? throw new DomainException("Projeto não encontrado.");

        project.Archive();

        await _repository.SaveChangesAsync();
    }

    private static ProjectResponseDto MapToDto(Project project)
    {
        return new ProjectResponseDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            OwnerId = project.OwnerId,
            OwnerName = project.Owner?.Name ?? string.Empty,
            Status = project.Status,
            CreatedAt = project.CreatedAt
        };
    }
}
