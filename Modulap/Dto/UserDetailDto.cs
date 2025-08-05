namespace Modulap.Dto
{
    public class UserDetailDto
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string[]? Roles { get; set; }
        public string? PhoneNumber { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int AccessFailedCount { get; set; }


        // Datos que usamos para nuestra base de datos

       

        public int Status {  get; set; }

        public string? LastName { get; set; }

        public string? address { get; set; } //domicilio

        public string? city { get; set; }

        public string? state { get; set; }
    }
}
