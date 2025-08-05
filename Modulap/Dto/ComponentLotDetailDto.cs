namespace Modulap.Dto
{
    public class ComponentLotDetailDto
    {
        public Guid IdComponentLot { get; set; }
        public Guid ComponentId { get; set; }
        public string ComponentName { get; set; }
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Amount { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
