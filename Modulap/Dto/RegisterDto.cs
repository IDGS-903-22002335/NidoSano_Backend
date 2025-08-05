using System.ComponentModel.DataAnnotations;

namespace Modulap.Dto
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]

        public string Email { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Datos que usamos para nuestra base de datos
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public int Status { get; set; } = 1;

        
        public string LastName { get; set; } = string.Empty;

       
        public string Address { get; set; } = string.Empty;

       
        public string City { get; set; } = string.Empty;

        
        public string PhoneNumber { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public List<string> Roles { get; set; }
    }
}
