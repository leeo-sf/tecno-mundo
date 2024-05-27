using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeekShopping.Identity.Data.ValueObjects
{
    public class UserVO
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(80, ErrorMessage = "{0} cannot be more than {2} characters")]
        public string UserName { get; set; }
        [StringLength(80, ErrorMessage = "{0} cannot be more than {2} characters")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(11, ErrorMessage = "{0} cannot be more than {2} characters")]
        public string Cpf { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(15, ErrorMessage = "{0} cannot be more than {2} characters")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string UserEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "{0} must be between 6 and 12 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
    }
}
