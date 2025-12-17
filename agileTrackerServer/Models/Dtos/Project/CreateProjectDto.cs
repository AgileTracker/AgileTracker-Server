using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace agileTrackerServer.Models.Dtos.Project;

[SwaggerSchema(Description = "DTO para criação de um projeto.")]
public class CreateProjectDto
{
    [SwaggerSchema("Nome do projeto.")]
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MinLength(3, ErrorMessage = "Nome deve ter no mínimo 3 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Descrição do projeto.")]
    public string Description { get; set; } = string.Empty;
}
