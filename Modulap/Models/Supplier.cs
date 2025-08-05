using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class Supplier
    {

        [Key]
        public Guid IdSupplier { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

        public int status { get; set; }

        // References to ComponentLot
        [JsonIgnore]
        public virtual ICollection<ComponentLot> ComponentLots { get; set; }

        //References to SuppliePayment
        [JsonIgnore]
        public virtual ICollection<SupplierPayment> SuppliersPayments { get; set; }


    }
}
