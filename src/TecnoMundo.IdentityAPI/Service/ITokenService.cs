using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Identity.Service
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
