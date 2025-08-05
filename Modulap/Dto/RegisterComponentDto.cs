namespace Modulap.Dto
{
    public class RegisterComponentDto
    {
        public Guid IdComponent { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public List<RecipeDetailCreateDto> Recipes { get; set; }
    }
}
