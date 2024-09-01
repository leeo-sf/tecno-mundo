using TecnoMundo.CartAPI.Data.ValueObjects;

namespace TecnoMundo.CartAPI.Command
{
    public interface ISaveOrUpdateCart
    {
        Task<CartVO> Execute(CartVO vo);
    }
}
