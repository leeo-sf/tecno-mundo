using GeekShopping.Identity.Model;
using GeekShopping.Identity.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Identity.Repository
{
    public class DbRepository : IDbRepository
    {
        private readonly MySQLContext _context;

        public DbRepository(MySQLContext context)
        {
            _context = context;
        }

        public async Task<User> ValidateUserEmailAndPassword(UserLogin user)
        {
            return await _context.Users
                .Include(x => x.Role)
                .Where(x => x.UserEmail == user.UserEmail && x.Password.Equals(user.Password, StringComparison.Ordinal))
                //.AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
