using GeekShopping.Identity.Data.ValueObjects;

namespace GeekShopping.Identity.Commands
{
    public interface IInsertUser
    {
        Task Execute(UserVO user);
    }
}
