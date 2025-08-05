using Modulap.Models;
using System.Text.Json.Serialization;

namespace Modulap.Dto
{
    public class ChickenCoopCreateDto
    {
        public Guid IdChickenCoop { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty ;

       

   

        public List<RecipeCreateDto> Recipes { get; set; }
    }
}
