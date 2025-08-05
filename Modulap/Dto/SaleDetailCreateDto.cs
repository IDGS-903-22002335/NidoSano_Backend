namespace Modulap.Dto
{
    public class SaleDetailCreateDto
    {
        public Guid IdSaleDetail { get; set; } = Guid.NewGuid();
        public Guid SaleId { get; set; }
        public int Amount { get; set; } // cantidad

        public decimal UnitPrice { get; set; } // precio Unitario
    }
}
