using System.ComponentModel.DataAnnotations;

namespace GeekShopping.Identity.Model
{
    public class UserLogin
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} is invalid")]
        public string UserEmail { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "{0} must be between 6 and 12 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
