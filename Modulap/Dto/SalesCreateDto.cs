using Modulap.Models;

namespace Modulap.Dto
{
    public class SalesCreateDto
    {
        public Guid IdSale { get; set; } = Guid.NewGuid();

        public string UserId { get; set; }



        public DateTime RegistrationDate { get; set; } = DateTime.Now;
      

        public TypeStatus Type { get; set; } = TypeStatus.Pendiente;
    }
}
