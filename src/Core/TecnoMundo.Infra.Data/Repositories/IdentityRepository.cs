using Microsoft.EntityFrameworkCore;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.Infra.Data.Context;

namespace TecnoMundo.Infra.Data.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly ApplicationDbContextIdentity _context;

        public IdentityRepository(ApplicationDbContextIdentity context)
        {
            _context = context;
        }

        public async Task<User?> ValidateUserEmailAndPassword(string email, string password)
        {
            return await _context
                .User.Where(x =>
                    x.UserEmail == email
                    && x.Password.Equals(password, StringComparison.Ordinal)
                )
                .AsNoTracking()
                .FirstOrDefaultAsync() ?? new User();
        }

        public async Task<bool> CpfExists(string cpf)
        {
            return await _context.User.Where(x => x.Cpf == cpf).AsNoTracking().AnyAsync();
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.User.Where(x => x.UserEmail == email).AsNoTracking().AnyAsync();
        }

        public async Task<bool> TelephoneExists(string phone)
        {
            return await _context
                .User.Where(x => x.PhoneNumber == phone)
                .AsNoTracking()
                .AnyAsync();
        }

        public async Task Create(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
