using System.ComponentModel.DataAnnotations;

namespace Modulap.Models
{
    public class PossibleClient
    {

        [Key]
        public Guid IdPossibleClient {  get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Status { get; set; }
    }
}
