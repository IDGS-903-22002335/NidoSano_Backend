using System.ComponentModel.DataAnnotations;

namespace Modulap.Dto
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string  Email { get; set; } = string.Empty;
    } 
}
