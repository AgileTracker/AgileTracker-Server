using agileTrackerServer.Models.Dtos.User;
using agileTrackerServer.Repositories.Interfaces;
using agileTrackerServer.Utils;

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

    public async Task<(ResponseUserDto user, string token)> LoginAsync(
        string email,
        string password)
    {
        var user = await _repository.GetByEmailAsync(email)
                   ?? throw new UnauthorizedAccessException("Credenciais inválidas.");

        if (!_hasher.VerifyPassword(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        var token = _tokenService.GenerateToken(user);

        return (new ResponseUserDto(user), token);
    }
}