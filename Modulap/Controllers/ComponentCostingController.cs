using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modulap.Data;

namespace Modulap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentCostingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ComponentCostingController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("GetAllCostings")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllCostings()
        {
            var costings = await _context.ComponentCostings
                .OrderByDescending(c => c.Date)
                .Select(c => new
                {
                    Fecha = c.Date,
                    Entrada = c.Entrance,
                    Salida = c.Exit,
                    Existencias = c.Existence,
                    Costo = c.cost,
                    Promedio = c.Average,
                    Debe = c.Owes,
                    Haber = c.ToHave,
                    Saldo = c.Balance
                })
                .ToListAsync();

            return Ok(costings);
        }

        [HttpGet("GetCostingsByComponentName/{componentName}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCostingsByComponentName(string componentName)
        {
            var costings = await _context.ComponentCostings
                .Include(c => c.ComponentLot)
                    .ThenInclude(cl => cl.Component) // Incluir el componente relacionado
                .Include(c => c.ComponentProduction)
                .Where(c =>
                    (c.ComponentLot != null &&
                     c.ComponentLot.Component != null &&
                     c.ComponentLot.Component.Name.ToLower() == componentName.ToLower())
                )
                .OrderBy(c => c.Date) // orden por fecha ascendente
                .Select(c => new
                {
                    Fecha = c.Date,
                    Entrada = c.Entrance,
                    Salida = c.Exit,
                    Existencias = c.Existence,
                    Costo = c.cost,
                    Promedio = c.Average,
                    Debe = c.Owes,
                    Haber = c.ToHave,
                    Saldo = c.Balance
                })
                .ToListAsync();

            if (!costings.Any())
            {
                return NotFound($"No se encontraron registros para el componente '{componentName}'");
            }

            return Ok(costings);
        }


    }
}
