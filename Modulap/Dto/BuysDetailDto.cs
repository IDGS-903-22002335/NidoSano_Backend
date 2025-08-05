namespace Modulap.Dto
{
    public class BuysDetailDto
    {
        public Guid IdBuys { get; set; }
        public string AdminId { get; set; }
        public string AdminFullName { get; set; }
        public DateTime DateBuys { get; set; }
        public decimal Total { get; set; }
        public List<ComponentLotDetailDto> ComponentLots { get; set; }
        }


}
