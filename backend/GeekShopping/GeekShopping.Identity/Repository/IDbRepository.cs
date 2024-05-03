using GeekShopping.Identity.Model;

namespace GeekShopping.Identity.Repository
{
    public interface IDbRepository
    {
        Task<User> ValidateUserEmailAndPassword(UserLogin userVO);
    }
}
