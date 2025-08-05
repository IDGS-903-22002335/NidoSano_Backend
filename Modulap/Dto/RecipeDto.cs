namespace Modulap.Dto
{
    public class RecipeDto
    {
        public Guid IdRecipe { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public List<RecipeDetailDto> RecipeDetail { get; set; }
    }
}
