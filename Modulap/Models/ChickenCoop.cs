using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modulap.Models
{
    public class ChickenCoop
    {


        [Key]
        public Guid IdChickenCoop { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]

        public Availability AvailabilityStatus { get; set; }

        // References to Message

        [JsonIgnore]
        public virtual ICollection<Message> Messages { get; set; }

        // References to Qualification
        [JsonIgnore]
        public virtual ICollection<Qualification> Qualifications { get; set; }

        // References to SaleDetail
        [JsonIgnore]
        public virtual ICollection<SaleDetail> SaleDetail { get; set; }

        //refence to Recipe
       // [JsonIgnore]
        public virtual ICollection<Recipe> Recipes { get; set; }

        // References  to Estimate
        [JsonIgnore]
        public virtual ICollection<Estimate> Estimates { get; set; } 
    }

    public enum Availability
    {
        Suficiente,
        Por_terminar,
        Bajo_Inventario
    }
}
