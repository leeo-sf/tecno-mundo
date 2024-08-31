using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Command
{
    public interface ISaveOrUpdateCart
    {
        Task<CartVO> Execute(CartVO vo);
    }
}
