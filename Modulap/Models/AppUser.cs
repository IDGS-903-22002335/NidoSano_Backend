using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Modulap.Models
{

    public class AppUser : IdentityUser
    {

        public string? FullName { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }


        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public DateTime FechaRegistro { get; set; }

        public int Status { get; set; }

        // References to Message

        [JsonIgnore]
        public virtual ICollection<Message> MessagesAsClients { get; set; }
        [JsonIgnore]
        public virtual ICollection<Message> MessagesAsAdministrators { get; set; }

        // Refences to Qualification
        [JsonIgnore]
        public virtual ICollection<Qualification> QualificationsAsClients { get; set; }


        // References to Sale
        [JsonIgnore]
        public virtual ICollection<Sale> Sales {get; set;}
      

        // References to Buys
        [JsonIgnore]
        public virtual ICollection<Buys> BuysAdmins { get; set; }

        [JsonIgnore]
        public virtual ICollection<Estimate> Estimates { get; set; }

    }

}
