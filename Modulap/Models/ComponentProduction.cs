using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class ComponentProduction
    {

        [Key]
        public Guid IdComponentProduction { get; set; }

        // Foreign key
        public Guid ComponentLotId { get; set; }
        public Guid ProductionLotId { get; set; }


        public int QuantityUsed { get; set; } // cantidad usada

        public DateTime Date {  get; set; }

        // Reference to ComponentLot
        public ComponentLot ComponentLot { get; set; }
        // Reference to ProductionLot
        public ProductionLot ProductionLot { get; set; }

        // Reference componentCosting
        [JsonIgnore]
        public virtual ICollection<ComponentCosting> ComponentCostings{ get; set; }

    }
}
