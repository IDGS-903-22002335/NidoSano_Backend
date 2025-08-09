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
    public class PossibleClientController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public PossibleClientController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("PossibleClientRegister")]
        public async Task<ActionResult<string>> Register( PossibleClientCreateDto possible)
        {
            var cliente = new PossibleClient
            {
                IdPossibleClient = possible.IdPossibleClient,
                Name = possible.Name,
                LastName = possible.LastName,
                PhoneNumber = possible.PhoneNumber,
                Email = possible.Email,
                City = possible.City,
                State = possible.State,
            };

            await _context.PossibleClients.AddAsync(cliente);
            await _context.SaveChangesAsync();

            return Ok( new {Message = $"Cliente {cliente.Name} registrado" });
        }

        [HttpGet("getAllPossibleClient")]
        public async Task<IActionResult> GetAll()
        {
            var posibleClient = await _context.PossibleClients
                .Where(c => c.Status != 1)   // Filtrar para excluir clientes con Status 1
                .ToListAsync();

            return Ok(posibleClient);
        }


        [HttpGet("getPossibleClientById/{id}")]
        public async Task<IActionResult> GetPossibleClientById(Guid id)
        {
            var cliente = await _context.PossibleClients.FindAsync(id);

            if (cliente == null)
            {
                return NotFound(new { Message = $"Cliente con id {id} no encontrado." });
            }

            return Ok(cliente);
        }


        [HttpPut("setStatusToOne/{id}")]
        public async Task<IActionResult> SetStatusToOne(Guid id)
        {
            var cliente = await _context.PossibleClients.FindAsync(id);

            if (cliente == null)
            {
                return NotFound(new { Message = $"Cliente con id {id} no encontrado." });
            }

            cliente.Status = 1;  // Actualizamos el Status a 1
            _context.PossibleClients.Update(cliente);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Status del cliente con id {id} actualizado a 1." });
        }


    }


}
