using AutoMapper;
using TecnoMundo.Identity.Data.ValueObjects;
using TecnoMundo.Identity.Model;
using TecnoMundo.Identity.Model.Context;
using Microsoft.EntityFrameworkCore;
using TecnoMundo.IdentityAPI.Data.ValueObjects;

namespace TecnoMundo.Identity.Repository
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

        public async Task<User> ValidateUserEmailAndPassword(AuthenticateVO user)
        {
            return await _context.Users
                .Where(x => x.UserEmail == user.UserEmail && x.Password.Equals(user.Password, StringComparison.Ordinal))
                //.AsNoTracking()
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

        public async Task Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
