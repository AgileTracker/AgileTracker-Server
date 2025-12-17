using agileTrackerServer.Models.Entities;
using agileTrackerServer.Repositories.Interfaces;
using agileTrackerServer.Models.Dtos.Project;
using agileTrackerServer.Models.Exceptions;


namespace agileTrackerServer.Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _repository;

        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync()
        {
            var projects = await _repository.GetAllAsync();
            return projects.Select(MapToDto);
        }

        public async Task<ProjectResponseDto?> GetByIdAsync(Guid id, Guid OwnerId)
        {
            var project = await _repository.GetByIdAsync(id, OwnerId);
            return project is null ? null : MapToDto(project);
        }

        public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto, Guid ownerId)
        {
            var project = new Project(dto.Name, dto.Description, ownerId);

            await _repository.AddAsync(project);
            await _repository.SaveChangesAsync();

            var created = await _repository.GetByIdAsync(project.Id, ownerId) ?? project;

            return MapToDto(created);
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
        private static ProjectResponseDto MapToDto(Project p)
        {
            return new ProjectResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                OwnerId = p.OwnerId,
                OwnerName = p.Owner?.Name ?? string.Empty,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
            };
        }
    }
}
