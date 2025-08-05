namespace Modulap.Dto
{
    public class BuysCreateDto
    {
        public Guid IdBuys { get; set; } = Guid.NewGuid();  

        public string AdminId { get; set; }

        public DateTime DateBuys { get; set; } = DateTime.Now;

        public Decimal Total { get; set; }

        public List<ComponentLotCreateDto> ComponentLots { get; set; }  // Lista de lotes comprados
    }
}
