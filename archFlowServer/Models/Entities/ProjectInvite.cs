using archFlowServer.Models.Enums;
using archFlowServer.Models.Exceptions;

namespace archFlowServer.Models.Entities;

public class ProjectInvite
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public MemberRole Role { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public InviteStatus Status { get; private set; }

    private ProjectInvite() { }

    public ProjectInvite(
        Guid projectId,
        string email,
        MemberRole role,
        TimeSpan expiration)
    {
        if (projectId == Guid.Empty)
            throw new ValidationException("Projeto inválido.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("Email inválido.");

        Id = Guid.NewGuid();
        ProjectId = projectId;
        Email = email.Trim().ToLowerInvariant();
        Role = role;
        Token = Guid.NewGuid().ToString("N");
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = CreatedAt.Add(expiration);
        Status = InviteStatus.Pending;
    }

    public void Accept()
    {
        if (Status == InviteStatus.Accepted)
            throw new ConflictException("Convite já foi aceito.");

        if (Status == InviteStatus.Declined)
            throw new ConflictException("Convite já foi recusado.");

        if (Status == InviteStatus.Revoked)
            throw new ConflictException("Convite foi revogado.");

        if (DateTime.UtcNow > ExpiresAt)
            throw new DomainException("Convite expirado.");

        Status = InviteStatus.Accepted;
    }


    public void Decline()
    {
        if (Status == InviteStatus.Accepted)
            throw new ConflictException("Convite já foi aceito.");

        if (Status == InviteStatus.Declined)
            throw new ConflictException("Convite já foi recusado.");

        if (Status == InviteStatus.Revoked)
            throw new ConflictException("Convite foi revogado.");

        if (DateTime.UtcNow > ExpiresAt)
            throw new DomainException("Convite expirado.");

        Status = InviteStatus.Declined;
    }

    public void Revoke()
    {
        if (Status == InviteStatus.Accepted)
            throw new ConflictException("Convite já foi aceito.");

        if (Status == InviteStatus.Declined)
            throw new ConflictException("Convite já foi recusado.");

        if (Status == InviteStatus.Revoked)
            throw new ConflictException("Convite foi revogado.");

        if (DateTime.UtcNow > ExpiresAt)
            throw new DomainException("Convite expirado.");

        Status = InviteStatus.Revoked;
    }


}

