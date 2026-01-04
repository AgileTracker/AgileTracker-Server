using System.ComponentModel.DataAnnotations;
using agileTrackerServer.Models.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace agileTrackerServer.Models.Dtos.Project;

[SwaggerSchema(Description = "DTO para adicionar um membro a um projeto.")]
public class AddProjectMemberDto
{
    [SwaggerSchema("ID do usuário que será adicionado ao projeto.")]
    [Required(ErrorMessage = "UserId é obrigatório.")]
    public Guid UserId { get; set; }

    [SwaggerSchema("Papel do usuário dentro do projeto.")]
    [Required(ErrorMessage = "Role é obrigatório.")]
    public MemberRole Role { get; set; }
}