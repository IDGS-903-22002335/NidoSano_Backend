using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class SaleDetail
    {

        [Key]
        public Guid IdSaleDetail { get; set; }
        public Guid SaleId { get; set; }

        public Guid? ChickenCoopId { get; set; } // id del producto

        public int Amount { get; set; } // cantidad

        public decimal UnitPrice { get; set; } // precio Unitario

        // Reference to sale
       public Sale Sale { get; set; }

        // Reference to ChickenCoop
       public ChickenCoop ChickenCoop { get; set; }


    }
}
