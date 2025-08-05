using Modulap.Models;

namespace Modulap.Dto
{
    public class UpdateSaleStatusDto
    {
        public Guid SaleId { get; set; }
        public TypeStatus NewStatus { get; set; }
    }
}
