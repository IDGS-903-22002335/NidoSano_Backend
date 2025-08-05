using System.ComponentModel.DataAnnotations;

namespace Modulap.Dto
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role Name is required")]

        public string RoleName { get; set; } = null!;

    }
}
