using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class ComponentLot
    {

        [Key]
        public Guid IdComponentLot { get; set; }
        public Guid ComponentId {  get; set; }
        public Guid BuysId { get; set; }
        public Guid SupplierId { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal AverageLot {  get; set; }
        public int Amount {  get; set; }

        public int AvailableQuantity { get; set; } // cantidad disponible

        // Refences to component

        public Component Component { get; set; }

        // Reference to Buys
        public Buys Buys { get; set; }

        //References to Supplier
        public Supplier Supplier { get; set; }

        // Reference to SupplierPayment
        [JsonIgnore]
        public virtual ICollection<SupplierPayment> SupplierPayments { get; set; }

        // Reference to ComponentProduction
        public virtual ICollection<ComponentProduction> ComponentProductions { get; set; }

        //Reference to ComponentCosting
        [JsonIgnore]
        public virtual ICollection<ComponentCosting> ComponentCostings { get; set; }
    }

}
