using agileTrackerServer.Models.Entities;
using agileTrackerServer.Models.Exceptions;

namespace agileTrackerServer.Models.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Active";
    public Guid OwnerId { get; private set; }

    public User? Owner { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private Project() { }

    public Project(string name, string? description, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do projeto é obrigatório.");

        if (ownerId == Guid.Empty)
            throw new DomainException("OwnerId inválido.");

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Status = "Active";
        OwnerId = ownerId;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do projeto é obrigatório.");

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
    }

    public void Archive()
    {
        Status = "Archived";
    }
}