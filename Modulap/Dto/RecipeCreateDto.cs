namespace Modulap.Dto
{
    public class RecipeCreateDto
    {
        public Guid IdRecipe { get; set; } = Guid.NewGuid();

        public Guid ChickenCoopId { get; set; }

        public int Amount { get; set; } = 0;// cantidad

        public string Description { get; set; } = string.Empty;


        public List<RecipeDetailCreateDto> Details { get; set; }
    }
}
