using TecnoMundo.Application.DTOs;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<User?> ValidateUserEmailAndPassword(string email, string password);
        Task<bool> CpfExists(string cpf);
        Task<bool> EmailExists(string email);
        Task<bool> TelephoneExists(string phone);
        Task Create(UserVO vo);
    }
}
