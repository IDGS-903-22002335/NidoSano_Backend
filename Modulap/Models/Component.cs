using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class Component
    {

        [Key] 
        public Guid IdComponent { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }

        public string Category { get; set; }

        // Reference to componentLot
        [JsonIgnore]
        public virtual ICollection<ComponentLot> ComponentLots { get; set; }

        // Reference to RecioeDetail
       // [JsonIgnore]
        public virtual ICollection<RecipeDetail> RecipeDetails { get; set; }

        // Reference to ComponentLoss
        [JsonIgnore]
        public virtual ICollection<ComponentLoss> ComponentLosses{ get; set; }
    }
}
