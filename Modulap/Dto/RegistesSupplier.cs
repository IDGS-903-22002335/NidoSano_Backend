namespace Modulap.Dto
{
    public class RegistesSupplier
    {
        public Guid IdSupplier { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; }= string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public int status { get; set; } = 1;
    }
}
