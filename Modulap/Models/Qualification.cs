using System.ComponentModel.DataAnnotations;

namespace Modulap.Models
{
    public class Qualification
    {

        [Key]
        public Guid IdQualification { get; set; }
        public string ClientId { get; set; }
        public Guid ChickenCoopId { get; set; }

        public int punctuation {get; set; }

        public DateTime Date { get; set; }

        //References
        public AppUser Client { get; set; }
        public ChickenCoop ChickenCoop { get; set; }

    }
}
