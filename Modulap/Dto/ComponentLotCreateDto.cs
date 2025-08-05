using System.ComponentModel.DataAnnotations;

namespace Modulap.Dto
{
    public class ComponentLotCreateDto
    {
        public Guid IdComponentLot { get; set; } = Guid.NewGuid();
        [Required]
        public Guid ComponentId { get; set; }

        [Required]
        public Guid SupplierId { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public int Amount { get; set; } = 0;
    }
}
