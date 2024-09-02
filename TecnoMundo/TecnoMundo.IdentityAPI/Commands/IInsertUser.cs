using TecnoMundo.Identity.Data.ValueObjects;

namespace TecnoMundo.Identity.Commands
{
    public interface IInsertUser
    {
        Task Execute(UserVO user);
    }
}
