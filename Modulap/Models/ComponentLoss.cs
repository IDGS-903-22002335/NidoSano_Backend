using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class ComponentLoss
    {

        [Key]
        public Guid IdComponentLoss { get; set; }

        public Guid ComponentId { get; set; }

        public int Amount { get; set; }

        public DateTime Date { get; set; }

        public TypeComponent Type {  get; set; }

        // Reference to Component
        public Component Component { get; set; }

        //Reference to ComponentCosting

        [JsonIgnore]
        public virtual ICollection<ComponentCosting> ComponentCostings { get; set; }


    }

    public enum TypeComponent
    {
        Danio,
        Defecto,
        Perdida
    }
}
