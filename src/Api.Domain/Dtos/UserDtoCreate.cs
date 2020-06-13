using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos
{
    public class UserDtoCreate
    {
        [Required(ErrorMessage="Name is required.")]
        [StringLength(65, ErrorMessage="Email must be at most {1} characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage="Email is required.")]
        [EmailAddress(ErrorMessage="Email is invalid.")]
        [StringLength(65, ErrorMessage="Email must be at most {1} characters.")]
        public string Email { get; set; }
    }
}