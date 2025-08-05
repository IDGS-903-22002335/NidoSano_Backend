using System.ComponentModel.DataAnnotations;

namespace Modulap.Models
{
    public class Message
    {


        [Key]
        public Guid IdMessage { get; set; }

        //claves foraneas
        public String? ClientId { get; set; }

        public String? AdministratorId { get; set; }
        public Guid ChickenCoopId { get; set; }

        public Guid? ParentMessageId { get; set; }
        //
        public TypeMessage TypeMessage { get; set; }

        public StatusMessage StatusMessage { get; set; }

        public String Text { get; set; }

        public DateTime Date { get; set; }


        public Message ParentMessage { get; set; }

        public ChickenCoop ChickenCoop { get; set; }
        public AppUser Administrator { get; set; }
        public AppUser Client { get; set; }

        public virtual ICollection<Message> ChildMessages { get; set; }


    }


    public enum TypeMessage
    {
        Pregunta,
        Respuesta,
        Comentario

    }

    public enum StatusMessage
    {
        Pendiente,
        Atendido
    }
}
