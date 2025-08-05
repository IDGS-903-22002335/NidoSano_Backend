namespace Modulap.Dto
{
    public class ComponentLotInfoDto
    {
        public Guid ComponentLotId { get; set; }
        public string ComponentName { get; set; }
        public int TotalAmount { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
