using System.ComponentModel.DataAnnotations;

namespace Modulap.Dto
{
    public class UpdateUserDto



    {
        [Required]
        public string Id { get; set; }
        [Required]
        [EmailAddress]

        public string Email { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Datos que usamos para nuestra base de datos

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

       
    }
}
