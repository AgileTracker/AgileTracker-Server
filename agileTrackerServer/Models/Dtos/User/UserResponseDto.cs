using Swashbuckle.AspNetCore.Annotations;

namespace agileTrackerServer.Models.Dtos.User;

[SwaggerSchema(Description = "Dados de retorno de um usuário.")]
public class UserResponseDto
{
    [SwaggerSchema("Identificador único do usuário.")]
    public Guid Id { get; set; }

    [SwaggerSchema("Nome do usuário.")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Email do usuário.")]
    public string Email { get; set; } = string.Empty;

    [SwaggerSchema("Tipo do usuário, representando sua assinatura.")]
    public string Type { get; set; } = string.Empty;

    [SwaggerSchema("URL do avatar do usuário.")]
    public string? AvatarUrl { get; set; }

    [SwaggerSchema("Data de criação do registro.")]
    public DateTime CreatedAt { get; set; }
    
    [SwaggerSchema("Data de atualização do registro.")]
    public DateTime UpdatedAt { get; set; }
}
