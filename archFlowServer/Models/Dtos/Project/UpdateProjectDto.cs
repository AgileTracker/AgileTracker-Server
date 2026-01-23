using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace archFlowServer.Models.Dtos.Project;

public class UpdateProjectDto
{
    [SwaggerSchema("Nome do projeto a ser atualizado.")]
    [Required(ErrorMessage = "Nome é obrigatório para atualizar.")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Descrição do projeto a ser atualizada.")]
    public string? Description { get; set; }
}

