using TecnoMundo.Identity.Data.ValueObjects;
using TecnoMundo.Identity.Model;
using TecnoMundo.IdentityAPI.Data.ValueObjects;

namespace TecnoMundo.Identity.Repository
{
    public interface IDbRepository
    {
        Task<User> ValidateUserEmailAndPassword(AuthenticateVO userVO);
        Task<bool> CpfExists(string cpf);
        Task<bool> EmailExists(string email);
        Task<bool> TelephoneExists(string phone);
        Task Create(User user);
    }
}
