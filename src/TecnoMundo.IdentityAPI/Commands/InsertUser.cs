using AutoMapper;
using TecnoMundo.Identity.Data.ValueObjects;
using TecnoMundo.Identity.Model;
using TecnoMundo.Identity.Repository;

namespace TecnoMundo.Identity.Commands
{
    public class InsertUser : IInsertUser
    {
        private readonly IDbRepository _repository;
        private readonly IMapper _mapper;

        public InsertUser(IDbRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Execute(UserVO userVO)
        {
            var user = _mapper.Map<User>(userVO);

            if (!User.ValidateCpf(user.Cpf)) throw new ArgumentException("CPF invalid.");

            if (!user.EmailConfirmed) throw new ArgumentException("Email was not confirmed.");

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
