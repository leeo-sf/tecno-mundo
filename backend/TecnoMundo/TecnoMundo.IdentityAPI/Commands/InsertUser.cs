using GeekShopping.Identity.Data.ValueObjects;
using GeekShopping.Identity.Model;
using GeekShopping.Identity.Repository;

namespace GeekShopping.Identity.Commands
{
    public class InsertUser : IInsertUser
    {
        private readonly IDbRepository _repository;

        public InsertUser(IDbRepository repository)
        {
            _repository = repository;
        }

        public async Task Execute(UserVO user)
        {
            if (!User.ValidateCpf(user.Cpf)) throw new ArgumentException("CPF invalid.");

            var role = await _repository.GetRoleById(user.RoleId);

            if (!user.EmailConfirmed) throw new ArgumentException("Email was not confirmed.");

            if (role is null) throw new KeyNotFoundException("Role id not found!");

            var cpfExists = await _repository.CpfExists(user.Cpf);
            if (cpfExists) throw new ApplicationException($"CPF {user.Cpf} already exists");

            var emailExists = await _repository.EmailExists(user.UserEmail);
            if (emailExists) throw new ApplicationException($"Email {user.UserEmail} already exists");

            var phoneExists = await _repository.TelephoneExists(user.PhoneNumber);
            if (phoneExists) throw new ApplicationException($"Phone Number {user.PhoneNumber} already exists");

            await _repository.Create(user);
        }
    }
}
