using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace agileTrackerServer.Models.Dtos.Backlog;

public class UpdateBacklogOverviewDto
{
    [SwaggerSchema("Overview do projeto será alterado.")]
    [Required(ErrorMessage = "Overview é obrigatório.")]
    public string? Overview { get; set; }
}