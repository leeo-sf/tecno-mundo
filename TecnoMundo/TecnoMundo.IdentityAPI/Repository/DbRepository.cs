using AutoMapper;
using GeekShopping.Identity.Data.ValueObjects;
using GeekShopping.Identity.Model;
using GeekShopping.Identity.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Identity.Repository
{
    public class DbRepository : IDbRepository
    {
        private readonly MySQLContext _context;
        private readonly IMapper _mapper;

        public DbRepository(MySQLContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> ValidateUserEmailAndPassword(UserLogin user)
        {
            return await _context.Users
                .Include(x => x.Role)
                .Where(x => x.UserEmail == user.UserEmail && x.Password.Equals(user.Password, StringComparison.Ordinal))
                //.AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            return await _context.Roles
                .Where(x => x.Id == roleId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CpfExists(string cpf)
        {
            return await _context.Users
                .Where(x => x.Cpf == cpf)
                .AsNoTracking()
                .AnyAsync();
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users
                .Where(x => x.UserEmail == email)
                .AsNoTracking()
                .AnyAsync();
        }

        public async Task<bool> TelephoneExists(string phone)
        {
            return await _context.Users
                .Where(x => x.PhoneNumber == phone)
                .AsNoTracking()
                .AnyAsync();
        }

        public async Task Create(UserVO userVO)
        {
            var user = _mapper.Map<User>(userVO);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
