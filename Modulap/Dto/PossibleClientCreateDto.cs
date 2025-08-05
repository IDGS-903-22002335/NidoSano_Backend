namespace Modulap.Dto
{
    public class PossibleClientCreateDto
    {
        public Guid IdPossibleClient { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty ;
    }
}
