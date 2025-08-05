namespace Modulap.Dto
{
    public class RecipeDetailCreateDto
    {
        public Guid IdRecipeDetail { get; set; } = Guid.NewGuid();

        public Guid RecipeId { get; set; }
        public Guid ComponentId { get; set; }

        public int ComponentQuantity { get; set; }
    }
}
