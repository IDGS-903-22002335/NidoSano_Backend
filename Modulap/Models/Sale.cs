using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class Sale
    {

        [Key]
        public Guid IdSale { get; set; }

        public string UserId { get; set; }

      

        public DateTime RegistrationDate { get; set; }
        public DateTime DeliveryDate { get; set; }

        public TypeStatus Type { get; set; }

        // References to AppUser
        public AppUser User { get; set; }

        public decimal TotalPrice { get; set; }

        public Guid? EstimateId { get; set; } // FK a Estimate

        public Estimate Estimate { get; set; } // Navegación



        // Reference to SaleDetail
        [JsonIgnore]
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }

        
    }


    public enum TypeStatus
    {
        Pendiente,
        Cancelado,
        Atendida,
        Enviado,
        Entregado
    }
}
