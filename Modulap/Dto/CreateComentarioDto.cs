using Modulap.Models;

namespace Modulap.Dto
{
    public class CreateComentarioDto
    {
        public string? ClientId { get; set; }

        public string? AdministratorId { get; set; }
        public Guid ChickenCoopId { get; set; }

        public Guid? ParentMessageId { get; set; }

        public string Text { get; set; }

        public TypeMessage TypeMessage { get; set; } = TypeMessage.Comentario;
    }
}
