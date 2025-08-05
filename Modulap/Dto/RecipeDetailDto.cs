namespace Modulap.Dto
{
    public class RecipeDetailDto
    {
        public Guid IdRecipeDetail { get; set; }
        public int ComponentQuantity { get; set; }
        public ComponentDto Component { get; set; }
    }
}
