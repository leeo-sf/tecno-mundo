using System.ComponentModel.DataAnnotations.Schema;
using TecnoMundo.Domain.Entities.Base;
using TecnoMundo.Domain.Enums;

namespace TecnoMundo.Domain.Entities
{
    [Table("user")]
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Cpf { get; set; }
        public string PhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public User(
            string userName,
            string lastName,
            string cpf,
            string phoneNumber,
            string userEmail,
            bool emailConfirmed,
            string password
        )
        {
            Id = Guid.NewGuid();
            UserName = userName;
            LastName = lastName;
            Cpf = cpf;
            PhoneNumber = phoneNumber;
            UserEmail = userEmail;
            EmailConfirmed = emailConfirmed;
            Password = password;
            Role = Role.Client;
        }

        public User()
        {
        }

        public static bool ValidateCpf(string cpf)
        {
            try
            {
                if (cpf.Length != 11 || AvoidSequence(cpf))
                {
                    return false;
                }

                string newCpf = cpf.Substring(0, 9);
                int total = 0;
                int reverse = 10;
                int sequence = 27;

                for (int i = 0; i <= sequence; i++)
                {
                    if (i > 8 && sequence == 18)
                    {
                        i -= 9;
                    }

                    total += Convert.ToInt32(newCpf[i].ToString()) * reverse;

                    reverse -= 1;
                    if (reverse < 2)
                    {
                        reverse = 11;
                        double digito = 11 - (total % 11);

                        if (digito > 9)
                        {
                            digito = 0;
                        }

                        newCpf += Convert.ToString(digito);
                        total = 0;
                    }
                    sequence -= 1;
                }

                if (!(newCpf == cpf))
                {
                    return false;
                }

                return true;
            }
            catch (FormatException e)
            {
                //capturando error de letras digitadas no campo cpf
                return false;
            }
        }

        private static bool AvoidSequence(string cpf)
        {
            switch (cpf)
            {
                case "11111111111":
                    return true;
                    break;
                case "22222222222":
                    return true;
                    break;
                case "33333333333":
                    return true;
                    break;
                case "44444444444":
                    return true;
                    break;
                case "55555555555":
                    return true;
                    break;
                case "66666666666":
                    return true;
                    break;
                case "77777777777":
                    return true;
                    break;
                case "88888888888":
                    return true;
                    break;
                case "99999999999":
                    return true;
                    break;
                case "00000000000":
                    return true;
                    break;
            }
            return false;
        }
    }
}