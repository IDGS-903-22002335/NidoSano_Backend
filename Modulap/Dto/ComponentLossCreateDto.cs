using Modulap.Models;

namespace Modulap.Dto
{
    public class ComponentLossCreateDto
    {
        public Guid IdComponentLoss { get; set; } = Guid.NewGuid();
        public Guid ComponentId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public TypeComponent Type { get; set; }

    }
}
