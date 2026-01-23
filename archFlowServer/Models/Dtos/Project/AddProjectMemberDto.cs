using System.ComponentModel.DataAnnotations;
using archFlowServer.Models.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace archFlowServer.Models.Dtos.Project;

[SwaggerSchema(Description = "DTO para adicionar um membro a um projeto.")]
public class AddProjectMemberDto
{
    [SwaggerSchema("ID do usuário que será adicionado ao projeto.")]
    [Required(ErrorMessage = "UserId é obrigatório.")]
    public Guid UserId { get; set; }

    [SwaggerSchema("Função do membro na equipe (exemplo: 'Owner', 'ScrumMaster', 'ProductOwner', 'Developer').")]
    [Required(ErrorMessage = "Role é obrigatÃ³rio.")]
    public MemberRole Role { get; set; }
}
