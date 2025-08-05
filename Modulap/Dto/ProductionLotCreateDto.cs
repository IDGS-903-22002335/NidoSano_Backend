using Modulap.Models;

namespace Modulap.Dto
{
    public class ProductionLotCreateDto
    {
        public Guid idProductionLot { get; set; } = Guid.NewGuid();

        public Guid RecipeId { get; set; } 

        public DateTime DateProduction { get; set; } = DateTime.Now;


        public Status Status { get; set; }
    }
}
