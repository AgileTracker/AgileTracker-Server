using agileTrackerServer.Models.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace agileTrackerServer.Models.Dtos.Project;

public class ProjectMemberResponseDto
{
    [SwaggerSchema("Id do usuário.")]
    public Guid UserId {get; set;}
    
    [SwaggerSchema("Nome completo do usuário.")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Email do usuário. Deve ser único.")]
    public string Email { get; set; } = string.Empty;
    
    [SwaggerSchema("Função do membro na equipe (exemplo: 'Owner', 'ScrumMaster', 'ProductOwner', 'Developer').")]
    public MemberRole Role { get; set; } = MemberRole.Developer;

    [SwaggerSchema("Senha em texto puro para cadastro. Será hasheada no servidor.")]
    public DateTime JoinedAt { get; set; }
    
   
}