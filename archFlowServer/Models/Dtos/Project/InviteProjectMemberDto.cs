using archFlowServer.Models.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace archFlowServer.Models.Dtos.Project;

[SwaggerSchema(Description = "DTO para convidar um novo membro para o projeto.")]
public class InviteProjectMemberDto
{
    [SwaggerSchema("Email do novo membro.")]
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [SwaggerSchema("Função do novo membro na equipe (exemplo: 'Owner', 'ScrumMaster', 'ProductOwner', 'Developer').")]
    [Required(ErrorMessage = "Role é obrigatório.")]
    public MemberRole Role { get; set; }
}

