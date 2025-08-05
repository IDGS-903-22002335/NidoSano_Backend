using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class RecipeDetail
    {

        [Key]
        public Guid IdRecipeDetail {  get; set; }

        public Guid RecipeId { get; set; }
        public Guid ComponentId {  get; set; }

        public int ComponentQuantity { get; set; }


        // Reference to Recipe
        public Recipe Recipe { get; set; }  

        public Component Component { get; set; }

        // Reference to ComponentCosting
        [JsonIgnore]
        public virtual ICollection<ComponentCosting> ComponentCostings { get; set; }
    }
}
