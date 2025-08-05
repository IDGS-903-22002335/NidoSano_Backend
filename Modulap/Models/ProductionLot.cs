using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class ProductionLot
    {

        [Key]
        public Guid idProductionLot { get; set; }

        public Guid RecipeId { get; set; }

        public DateTime DateProduction { get; set; }

        public int AvailableQuantity { get; set; } // cantidad dispobles

        public Status Status { get; set; }
    
        // Reference to Recipe
        public Recipe Recipe { get; set; }

        // reference to ComponentProduction
        [JsonIgnore]
        public virtual ICollection<ComponentProduction> ComponentProductions { get; set; }
    }


    public enum Status
    {
        Pendiente,
        Cancelado,
        Produciendo,
        Terminado
    }
}
