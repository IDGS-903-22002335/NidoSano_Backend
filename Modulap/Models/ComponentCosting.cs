using System.ComponentModel.DataAnnotations;

namespace Modulap.Models
{
    public class ComponentCosting
    {
        [Key]
        public Guid IdComponentCosting { get; set; }

        public Guid? ComponentLossId { get; set; }

        public Guid? RecipeDetailId {  get; set; }

        public Guid? ComponentLotId {  get; set; }

        public Guid? ComponentProductionId { get; set; }

        public DateTime Date {  get; set; }

        public int Entrance { get; set; }

        public int Exit {  get; set; }

        public int Existence { get; set; }

        public decimal cost { get; set; }

        public decimal Average { get; set; } // promedio

        public decimal Owes { get; set; } // debes

        public decimal ToHave { get; set; } 

        public decimal Balance { get; set; }

        // reference to Component Loss
        public ComponentLoss ComponentLoss { get; set; }

        // refrence to Coponent RecipeDetail

        public RecipeDetail RecipeDetail { get; set; }

        // Reference to ComponentLot
        public ComponentLot ComponentLot { get; set; }

        // Reference to  ConponentCosting
        public ComponentProduction ComponentProduction { get; set; }




}
}
