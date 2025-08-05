using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modulap.Data;
using Modulap.Dto;
using Modulap.Models;

namespace Modulap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MessageController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreateMessage([FromBody] CreateComentarioDto dto)
        {
            if (string.IsNullOrEmpty(dto.Text))
                return BadRequest("El texto es obligatorio.");

            if (string.IsNullOrEmpty(dto.ClientId) && string.IsNullOrEmpty(dto.AdministratorId))
                return BadRequest("Debe especificar el cliente o el administrador que envía el mensaje.");

            // Verifica si el mensaje es una respuesta, o si es un comentario creado por un administrador
            var status = (dto.TypeMessage == TypeMessage.Respuesta ||
                         (dto.TypeMessage == TypeMessage.Comentario && !string.IsNullOrEmpty(dto.AdministratorId)))
                ? StatusMessage.Atendido
                : StatusMessage.Pendiente;

            var message = new Message
            {
                IdMessage = Guid.NewGuid(),
                ClientId = dto.ClientId,
                AdministratorId = dto.AdministratorId,
                ChickenCoopId = dto.ChickenCoopId,
                ParentMessageId = dto.ParentMessageId,
                Text = dto.Text,
                TypeMessage = dto.TypeMessage,
                StatusMessage = status,
                Date = DateTime.UtcNow
            };

            _context.Messages.Add(message);

            if (dto.ParentMessageId.HasValue)
            {
                var mensajePadre = await _context.Messages.FindAsync(dto.ParentMessageId.Value);

                if (mensajePadre != null)
                {
                    // Marcar mensaje padre como atendido si aplica
                    if ((mensajePadre.TypeMessage == TypeMessage.Pregunta && dto.TypeMessage == TypeMessage.Respuesta) ||
                        (mensajePadre.TypeMessage == TypeMessage.Comentario && dto.TypeMessage == TypeMessage.Comentario))
                    {
                        mensajePadre.StatusMessage = StatusMessage.Atendido;
                        _context.Messages.Update(mensajePadre);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Mensaje creado correctamente", messageId = message.IdMessage });
        }


        [HttpPost("CreateQualification")]
        public async Task<IActionResult> CreateQualification([FromBody] CreateQualificationDto dto)
        {
            if (string.IsNullOrEmpty(dto.ClientId) || dto.Punctuation < 1 || dto.Punctuation > 5)
            {
                return BadRequest("Debe proporcionar un cliente válido y una puntuación entre 1 y 5.");
            }

            var qualification = new Qualification
            {
                IdQualification = Guid.NewGuid(),
                ClientId = dto.ClientId,
                ChickenCoopId = dto.ChickenCoopId,
                punctuation = dto.Punctuation,
                Date = DateTime.UtcNow
            };

            _context.Qualifications.Add(qualification);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Calificación registrada correctamente", qualificationId = qualification.IdQualification });
        }

        // Clientes
        [HttpGet("ComentariosConRespuestas")]
        public async Task<IActionResult> GetComentariosConRespuestas()
        {
            var comentarios = await _context.Messages
                .Where(m => m.TypeMessage == TypeMessage.Comentario)
                .Select(m => new
                {
                    Id = m.IdMessage,
                    Text = m.Text,
                    Date = m.Date,
                    ClientId = m.ClientId,
                    ClientFullName = m.Client.FullName, 
                    AdministratorId = m.AdministratorId,
                    ChickenCoopId = m.ChickenCoopId,
                    Status = m.StatusMessage,

                    Calificacion = _context.Qualifications
                        .Where(q => q.ClientId == m.ClientId && q.ChickenCoopId == m.ChickenCoopId)
                        .Select(q => q.punctuation)
                        .FirstOrDefault(),

                    Respuestas = m.ChildMessages
                        .Where(c => c.TypeMessage == TypeMessage.Comentario || c.TypeMessage == TypeMessage.Respuesta)
                        .Select(c => new
                        {
                            Id = c.IdMessage,
                            Text = c.Text,
                            Date = c.Date,
                            ClientId = c.ClientId,
                            ClientFullName = c.Client.FullName, 
                            AdministratorId = c.AdministratorId,
                            Type = c.TypeMessage,
                            Status = c.StatusMessage
                        }).ToList()
                })
                .ToListAsync();

            return Ok(comentarios);
        }



        [HttpGet("PreguntasyComentariosPendientes")]
        public async Task<IActionResult> GetPreguntasPendientes()
        {
            var mensajesPendientes = await _context.Messages
                .Where(m =>
                    (m.TypeMessage == TypeMessage.Pregunta || m.TypeMessage == TypeMessage.Comentario) &&
                    m.StatusMessage == StatusMessage.Pendiente
                )
                .Select(m => new
                {
                    Id = m.IdMessage,
                    Text = m.Text,
                    Date = m.Date,
                    ClientId = m.ClientId,
                    ClientName = m.Client.FullName, // Aquí agregas el nombre
                    AdministratorId = m.AdministratorId,
                    AdministratorName = m.Administrator.FullName, // También si lo necesitas
                    ChickenCoopId = m.ChickenCoopId,
                    TypeMessage = m.TypeMessage,
                    Status = m.StatusMessage,

                    ParentMessage = m.ParentMessage != null ? new
                    {
                        Id = m.ParentMessage.IdMessage,
                        Text = m.ParentMessage.Text,
                        TypeMessage = m.ParentMessage.TypeMessage,
                        Status = m.ParentMessage.StatusMessage,
                        ClientName = m.ParentMessage.Client.FullName // También puedes incluir esto
                    } : null
                })
                .ToListAsync();

            return Ok(mensajesPendientes);
        }



        // para los clientes
        [HttpGet("PreguntasConRespuestas")]
        public async Task<IActionResult> GetPreguntasConRespuestas()
        {
            var preguntasConRespuestas = await _context.Messages
                .Where(m => m.TypeMessage == TypeMessage.Pregunta && m.ParentMessageId == null)
                .Select(m => new
                {
                    Id = m.IdMessage,
                    Text = m.Text,
                    Date = m.Date,
                    ClientId = m.ClientId,
                    ClientFullName = m.Client.FullName,

                    AdministratorId = m.AdministratorId,
                    ChickenCoopId = m.ChickenCoopId,
                    Status = m.StatusMessage,
                    Respuestas = m.ChildMessages
                        .Where(c => c.TypeMessage == TypeMessage.Respuesta)
                        .Select(c => new
                        {
                            Id = c.IdMessage,
                            Text = c.Text,
                            Date = c.Date,
                            ClientId = c.ClientId,
                            AdministratorId = c.AdministratorId,
                            Status = c.StatusMessage
                        }).ToList()
                })
                .ToListAsync();

            return Ok(preguntasConRespuestas);
        }


        //Admin
        [HttpGet("Pregunta/{id}")]
        public async Task<IActionResult> GetPreguntaPorId(Guid id)
        {
            var pregunta = await _context.Messages
                .Where(m => m.IdMessage == id && m.TypeMessage == TypeMessage.Pregunta)
                .Select(m => new
                {
                    Id = m.IdMessage,
                    Text = m.Text,
                    Date = m.Date,
                    ClientId = m.ClientId,
                    AdministratorId = m.AdministratorId,
                    ChickenCoopId = m.ChickenCoopId,
                    Status = m.StatusMessage,
                    Respuestas = m.ChildMessages
                        .Where(c => c.TypeMessage == TypeMessage.Respuesta)
                        .Select(c => new
                        {
                            Id = c.IdMessage,
                            Text = c.Text,
                            Date = c.Date,
                            ClientId = c.ClientId,
                            AdministratorId = c.AdministratorId,
                            Status = c.StatusMessage
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (pregunta == null)
                return NotFound("Pregunta no encontrada.");

            return Ok(pregunta);
        }
        [HttpGet("Comentario/{id}")]
        public async Task<IActionResult> GetComentarioPorId(Guid id)
        {
            var comentario = await _context.Messages
                .Where(m => m.IdMessage == id && m.TypeMessage == TypeMessage.Comentario)
                .Select(m => new
                {
                    Id = m.IdMessage,
                    Text = m.Text,
                    Date = m.Date,
                    ClientId = m.ClientId,
                    AdministratorId = m.AdministratorId,
                    ChickenCoopId = m.ChickenCoopId,
                    Status = m.StatusMessage,
                    Respuestas = m.ChildMessages
                        .Where(c => c.TypeMessage == TypeMessage.Comentario)
                        .Select(c => new
                        {
                            Id = c.IdMessage,
                            Text = c.Text,
                            Date = c.Date,
                            ClientId = c.ClientId,
                            AdministratorId = c.AdministratorId,
                            Status = c.StatusMessage
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (comentario == null)
                return NotFound("Comentario no encontrado.");

            return Ok(comentario);
        }

        [HttpGet("Calificacion")]
        public async Task<IActionResult> GetCalificacionPorClienteYGallinero([FromQuery] string clientId, [FromQuery] Guid chickenCoopId)
        {
            if (string.IsNullOrEmpty(clientId) || chickenCoopId == Guid.Empty)
                return BadRequest("Debe proporcionar el ClientId y el ChickenCoopId.");

            var calificacion = await _context.Qualifications
                .Where(q => q.ClientId == clientId && q.ChickenCoopId == chickenCoopId)
                .Select(q => new
                {
                    q.IdQualification,
                    q.punctuation,
                    q.Date
                })
                .FirstOrDefaultAsync();

            if (calificacion == null)
                return NotFound("No se encontró calificación para este cliente y gallinero.");

            return Ok(calificacion);
        }


        [HttpGet("ResumenCalificaciones/{chickenCoopId}")]
        public async Task<IActionResult> GetResumenCalificaciones(Guid chickenCoopId)
        {
            var calificaciones = await _context.Qualifications
                .Where(q => q.ChickenCoopId == chickenCoopId)
                .ToListAsync();

            if (calificaciones == null || !calificaciones.Any())
                return NotFound("No se encontraron calificaciones para este gallinero.");

            var total = calificaciones.Count;

            var resumenPorPuntuacion = calificaciones
                .GroupBy(q => q.punctuation)
                .Select(g => new
                {
                    Puntuacion = g.Key,
                    Conteo = g.Count()
                })
                .OrderBy(r => r.Puntuacion)
                .ToList();

            var promedio = calificaciones.Average(q => q.punctuation);

            return Ok(new
            {
                TotalCalificaciones = total,
                Promedio = Math.Round(promedio, 2),
                DetallePorPuntuacion = resumenPorPuntuacion
            });
        }


    }
}