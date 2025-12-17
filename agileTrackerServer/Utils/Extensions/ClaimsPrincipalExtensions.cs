using System.Security.Claims;

namespace agileTrackerServer.Utils.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claimValue = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(claimValue))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        if (!Guid.TryParse(claimValue, out var userId))
            throw new UnauthorizedAccessException("Identificador de usuário inválido.");

        return userId;
    }
}