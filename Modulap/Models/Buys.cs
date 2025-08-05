using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class Buys
    {

        [Key]
        public Guid IdBuys { get; set; }

        public string AdminId { get; set; }

        public DateTime DateBuys { get; set; }

        public Decimal Total {  get; set; }

        // Reference to AppUser
        public AppUser Administrator { get; set; }

        // Refrence to ComponenteLot
        [JsonIgnore]
        public virtual ICollection<ComponentLot> ComponentLots { get; set; }
    }
}
