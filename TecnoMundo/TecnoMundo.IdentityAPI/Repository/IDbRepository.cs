using GeekShopping.Identity.Data.ValueObjects;
using GeekShopping.Identity.Model;

namespace GeekShopping.Identity.Repository
{
    public interface IDbRepository
    {
        Task<User> ValidateUserEmailAndPassword(UserLogin userVO);
        Task<Role> GetRoleById(int roleId);
        Task<bool> CpfExists(string cpf);
        Task<bool> EmailExists(string email);
        Task<bool> TelephoneExists(string phone);
        Task Create(UserVO user);
    }
}
