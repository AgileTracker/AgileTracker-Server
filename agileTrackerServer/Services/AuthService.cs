using agileTrackerServer.Models.Dtos.User;
using agileTrackerServer.Models.Enums;
using agileTrackerServer.Models.Exceptions;
using agileTrackerServer.Repositories.Interfaces;
using agileTrackerServer.Utils;
using System.Security.Claims;
using agileTrackerServer.Utils.Extensions;

namespace agileTrackerServer.Services;

public class AuthService
{
    private readonly IUserRepository _repository;
    private readonly PasswordHasher _hasher;
    private readonly TokenService _tokenService;

    public AuthService(
        IUserRepository repository,
        PasswordHasher hasher,
        TokenService tokenService)
    {
        _repository = repository;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    // ============================
    // LOGIN
    // ============================
    public async Task<(ResponseUserDto user, string token)> LoginAsync(
        string email,
        string password)
    {
        var user = await _repository.GetByEmailAsync(email)
                   ?? throw new DomainException("Credenciais inválidas.");

        if (!_hasher.VerifyPassword(password, user.PasswordHash))
            throw new DomainException("Credenciais inválidas.");

        var token = _tokenService.GenerateToken(user);

        return (new ResponseUserDto(user), token);
    }

    // ============================
    // ME
    // ============================
    public ResponseUserDto GetAuthenticatedUser(ClaimsPrincipal user)
    {
        var userId = user.GetUserId();

        var typeClaim = user.FindFirstValue("Type")
                        ?? throw new DomainException("Tipo do usuário não encontrado no token.");

        if (!Enum.TryParse<UserType>(typeClaim, true, out var userType))
            throw new DomainException("Tipo do usuário inválido.");

        return new ResponseUserDto
        {
            Id = userId,
            Name = user.FindFirstValue("Name") ?? string.Empty,
            Email = user.FindFirstValue("Email") ?? string.Empty,
            Type = userType
        };
    }
}