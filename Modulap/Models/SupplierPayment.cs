using System.ComponentModel.DataAnnotations;

namespace Modulap.Models
{
    public class SupplierPayment
    {


        [Key]
        public Guid IdSupplierPayment { get; set; }
        
        
        public Guid SupplierId { get; set; }

        public Guid ComponentLotId { get; set; }

        public DateTime Date { get; set; }
        
        public decimal amount { get; set; }

        // References to Supplier
        public Supplier Supplier { get; set; }

        // References to ComponentLot
        public ComponentLot ComponentLot { get; set; }


    }
}
