using AutoMapper;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;

namespace TecnoMundo.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _repository;
        private readonly IMapper _mapper;

        public IdentityService(IIdentityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> CpfExists(string cpf)
        {
            return await _repository.CpfExists(cpf);
        }

        public async Task Create(UserVO vo)
        {
            var user = _mapper.Map<User>(vo);

            if (!User.ValidateCpf(user.Cpf))
                throw new ArgumentException("CPF invalid.");

            if (!user.EmailConfirmed)
                throw new ArgumentException("Email was not confirmed.");

            var cpfExists = await _repository.CpfExists(user.Cpf);
            if (cpfExists)
                throw new ApplicationException($"CPF {user.Cpf} already exists");

            var emailExists = await _repository.EmailExists(user.UserEmail);
            if (emailExists)
                throw new ApplicationException($"Email {user.UserEmail} already exists");

            var phoneExists = await _repository.TelephoneExists(user.PhoneNumber);
            if (phoneExists)
                throw new ApplicationException($"Phone Number {user.PhoneNumber} already exists");

            await _repository.Create(user);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _repository.EmailExists(email);
        }

        public async Task<bool> TelephoneExists(string phone)
        {
            return await _repository.TelephoneExists(phone);
        }

        public async Task<User?> ValidateUserEmailAndPassword(string email, string password)
        {
            var user =
                await _repository.ValidateUserEmailAndPassword(email, password) ?? new User();
            user.Password = "";
            return user;
        }
    }
}
