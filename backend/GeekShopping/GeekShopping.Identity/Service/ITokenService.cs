using GeekShopping.Identity.Model;

namespace GeekShopping.Identity.Service
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
