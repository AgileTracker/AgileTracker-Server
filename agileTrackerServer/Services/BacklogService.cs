using agileTrackerServer.Models.Dtos.Backlog;
using agileTrackerServer.Models.Entities;
using agileTrackerServer.Models.Exceptions;
using agileTrackerServer.Repositories.Interfaces;

namespace agileTrackerServer.Services;

public class BacklogService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProductBacklogRepository _backlogRepository;
    private readonly IEpicRepository _epicRepository;
    private readonly IUserStoryRepository _storyRepository;

    public BacklogService(
        IProjectRepository projectRepository,
        IProductBacklogRepository backlogRepository,
        IEpicRepository epicRepository,
        IUserStoryRepository storyRepository)
    {
        _projectRepository = projectRepository;
        _backlogRepository = backlogRepository;
        _epicRepository = epicRepository;
        _storyRepository = storyRepository;
    }

    public async Task CreateEpicAsync(Guid projectId, Guid userId, CreateEpicDto dto)
    {
        _ = await _projectRepository.GetByIdAsync(projectId, userId)
            ?? throw new NotFoundException("Projeto não encontrado.");

        var backlog = await _backlogRepository.GetByProjectIdAsync(projectId)
            ?? throw new DomainException("Product backlog não encontrado para este projeto.");

        // ✅ Position é sempre calculado no backend (append)
        var nextPosition = await _epicRepository.GetNextPositionAsync(backlog.Id);

        var epic = new Epic(
            backlog.Id,
            dto.Name,
            dto.Description,
            dto.BusinessValue,
            dto.Status,
            position: nextPosition,
            dto.Priority,
            dto.Color
        );

        await _epicRepository.AddAsync(epic);
        await _epicRepository.SaveChangesAsync();
    }

    public async Task CreateUserStoryAsync(Guid projectId, int epicId, Guid userId, CreateUserStoryDto dto)
    {
        _ = await _projectRepository.GetByIdAsync(projectId, userId)
            ?? throw new NotFoundException("Projeto não encontrado.");

        var epic = await _epicRepository.GetByIdAsync(epicId)
            ?? throw new NotFoundException("Épico não encontrado.");

        var backlog = await _backlogRepository.GetByProjectIdAsync(projectId)
            ?? throw new DomainException("Product backlog não encontrado para este projeto.");

        if (epic.ProductBacklogId != backlog.Id)
            throw new UnauthorizedAccessException("Este épico não pertence ao projeto informado.");

        // ✅ Position é sempre calculado no backend (append)
        var nextPosition = await _storyRepository.GetNextPositionAsync(epicId);

        var story = new UserStory(
            epicId,
            dto.Title,
            dto.Persona,
            dto.Description,
            dto.AcceptanceCriteria,
            dto.Complexity,
            dto.Effort,
            dto.Dependencies,
            dto.Priority,
            dto.BusinessValue,
            dto.Status,
            position: nextPosition,
            dto.AssigneeId
        );

        await _storyRepository.AddAsync(story);
        await _storyRepository.SaveChangesAsync();
    }

    public async Task<ProductBacklogResponseDto> GetBacklogByProjectIdAsync(Guid projectId, Guid userId)
    {
        _ = await _projectRepository.GetByIdAsync(projectId, userId)
            ?? throw new NotFoundException("Projeto não encontrado.");

        var backlog = await _backlogRepository.GetByProjectIdAsync(projectId)
            ?? throw new DomainException("Product backlog não encontrado para este projeto.");

        var dto = new ProductBacklogResponseDto
        {
            Id = backlog.Id,
            ProjectId = backlog.ProjectId,
            Overview = backlog.Overview,
            CreatedAt = backlog.CreatedAt,
            UpdatedAt = backlog.UpdatedAt,

            // ✅ Ordenação por Position (UI) primeiro
            Epics = backlog.Epics
                .OrderBy(e => e.Position)
                .ThenByDescending(e => e.Priority)
                .ThenBy(e => e.CreatedAt)
                .Select(e => new EpicResponseDto
                {
                    Id = e.Id,
                    ProductBacklogId = e.ProductBacklogId,
                    Name = e.Name,
                    Description = e.Description,
                    BusinessValue = e.BusinessValue,
                    Status = e.Status,
                    Position = e.Position,
                    Priority = e.Priority,
                    Color = e.Color,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,

                    UserStories = e.UserStories
                        .OrderBy(s => s.Position)
                        .ThenByDescending(s => s.Priority)
                        .ThenBy(s => s.CreatedAt)
                        .Select(s => new UserStoryResponseDto
                        {
                            Id = s.Id,
                            EpicId = s.EpicId,
                            Title = s.Title,
                            Persona = s.Persona,
                            Description = s.Description,
                            AcceptanceCriteria = s.AcceptanceCriteria,
                            Complexity = s.Complexity,
                            Effort = s.Effort,
                            Position = s.Position,
                            Dependencies = s.Dependencies,
                            Priority = s.Priority,
                            BusinessValue = s.BusinessValue,
                            Status = s.Status,
                            AssigneeId = s.AssigneeId,
                            CreatedAt = s.CreatedAt,
                            UpdatedAt = s.UpdatedAt
                        })
                        .ToList()
                })
                .ToList()
        };

        return dto;
    }

    public async Task UpdateOverviewAsync(Guid projectId, Guid userId, UpdateBacklogOverviewDto dto)
    {
        _ = await _projectRepository.GetByIdAsync(projectId, userId)
            ?? throw new NotFoundException("Projeto não encontrado.");

        var backlog = await _backlogRepository.GetByProjectIdAsync(projectId)
            ?? throw new DomainException("Product backlog não encontrado para este projeto.");

        backlog.UpdateOverview(dto.Overview);

        await _backlogRepository.SaveChangesAsync();
    }

    public async Task UpdateEpicAsync(Guid projectId, int epicId, Guid userId, UpdateEpicDto dto)
    {
        _ = await _projectRepository.GetByIdAsync(projectId, userId)
            ?? throw new NotFoundException("Projeto não encontrado.");

        var backlog = await _backlogRepository.GetByProjectIdAsync(projectId)
            ?? throw new DomainException("Product backlog não encontrado para este projeto.");

        var epic = await _epicRepository.GetByIdAsync(epicId)
            ?? throw new NotFoundException("Épico não encontrado.");

        if (epic.ProductBacklogId != backlog.Id)
            throw new UnauthorizedAccessException("Este épico não pertence ao projeto informado.");

        epic.Update(
            name: dto.Name ?? epic.Name,
            description: dto.Description ?? epic.Description,
            businessValue: dto.BusinessValue ?? epic.BusinessValue,
            status: dto.Status ?? epic.Status,
            position: dto.Position ?? epic.Position,
            priority: dto.Priority ?? epic.Priority,
            color: dto.Color ?? epic.Color
        );

        await _epicRepository.SaveChangesAsync();
    }

    public async Task UpdateUserStoryAsync(Guid projectId, int storyId, Guid userId, UpdateUserStoryDto dto)
    {
        _ = await _projectRepository.GetByIdAsync(projectId, userId)
            ?? throw new NotFoundException("Projeto não encontrado.");

        var backlog = await _backlogRepository.GetByProjectIdAsync(projectId)
            ?? throw new DomainException("Product backlog não encontrado para este projeto.");

        var story = await _storyRepository.GetByIdWithEpicAsync(storyId)
            ?? throw new NotFoundException("User story não encontrada.");

        if (story.Epic.ProductBacklogId != backlog.Id)
            throw new UnauthorizedAccessException("Esta user story não pertence ao projeto informado.");

        story.Update(
            title: dto.Title ?? story.Title,
            persona: dto.Persona ?? story.Persona,
            description: dto.Description ?? story.Description,
            acceptanceCriteria: dto.AcceptanceCriteria ?? story.AcceptanceCriteria,
            complexity: dto.Complexity ?? story.Complexity,
            effort: dto.Effort ?? story.Effort,
            dependencies: dto.Dependencies ?? story.Dependencies,
            priority: dto.Priority ?? story.Priority,
            businessValue: dto.BusinessValue ?? story.BusinessValue,
            status: dto.Status ?? story.Status,
            position: dto.Position ?? story.Position,
            assigneeId: dto.AssigneeId ?? story.AssigneeId
        );

        await _storyRepository.SaveChangesAsync();
    }
}
