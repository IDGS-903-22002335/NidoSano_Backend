using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class Recipe
    {

        [Key]
        public Guid IdRecipe { get; set; }

        public Guid ChickenCoopId { get; set; }

        public int Amount {  get; set; } // cantidad

        public string Description { get; set; }

        // refence to chichkenCoop

      public ChickenCoop ChickenCoop { get; set; }

        // Refence to RecipeDetail

        //[JsonIgnore]
        public virtual ICollection<RecipeDetail> RecipeDetail { get; set; }

        // Refence to ProductionLot
        public virtual ICollection<ProductionLot>  ProductionLots{ get; set; }


    }
}
