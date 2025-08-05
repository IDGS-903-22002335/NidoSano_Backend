using System.Text.Json.Serialization;

namespace Modulap.Dto
{
    public class ChickenCoopWithRecipesDto
    {
        public Guid IdChickenCoop { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

       
        public string AvailabilityStatus { get; set; }
        public List<RecipeDto> Recipes { get; set; }
    }
}
