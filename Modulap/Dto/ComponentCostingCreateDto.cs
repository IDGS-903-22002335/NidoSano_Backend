namespace Modulap.Dto
{
    public class ComponentCostingCreateDto
    {
        public Guid IdComponentCosting { get; set; } = Guid.NewGuid();

     

        public Guid ComponentLotId { get; set; }

      

        public DateTime Date { get; set; } = DateTime.Now;
        public int Entrance { get; set; } = 0;

        public int Exit { get; set; } = 0;

        public int Existence { get; set; } = 0;

        public decimal cost { get; set; } = 0;

        public decimal Average { get; set; } = 0;// promedio

        public decimal Owes { get; set; } = 0;// debes

        public decimal ToHave { get; set; } = 0;

        public decimal Balance { get; set; } = 0;
    }
}
