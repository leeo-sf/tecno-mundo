using TecnoMundo.Identity.Model;

namespace TecnoMundo.Identity.Service
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
