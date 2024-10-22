using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Domain.Interfaces
{
    public interface IIdentityRepository
    {
        Task<User?> ValidateUserEmailAndPassword(string email, string password);
        Task<bool> CpfExists(string cpf);
        Task<bool> EmailExists(string email);
        Task<bool> TelephoneExists(string phone);
        Task Create(User user);
    }
}
