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
    public class ComponentController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ComponentController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        //Guardar
        [HttpPost("RegisterComponent")]
        public async Task<IActionResult> RegisterComponent([FromBody] ComponentDto dto)
        {
           
     

                // Crear nuevo componente
                var component = new Component
                {
                    IdComponent = Guid.NewGuid(),
                    Name = dto.Name.Trim(),
                    Description = dto.Description?.Trim(),
                    Category = dto.Category.Trim(),
              
                };

                _context.Components.Add(component);
                await _context.SaveChangesAsync();

            
            return Ok(new {Message = $"Component {dto.Name} Guardado correctamente"});
     
        }

        [HttpPost("RegisterLoss")]
        public async Task<IActionResult> RegisterLoss([FromBody] RegisterLossDto dto)
        {
            // Validaciones iniciales
            var lote = await _context.ComponentLots
                .Include(cl => cl.Component)
                .FirstOrDefaultAsync(cl => cl.IdComponentLot == dto.IdComponentLot);

            if (lote == null) return NotFound("Lote no encontrado.");
            if (dto.Amount > lote.AvailableQuantity)
                return BadRequest("No hay suficiente cantidad disponible en el lote.");
            if (!Enum.TryParse<TypeComponent>(dto.Type, true, out var parsedType))
                return BadRequest("Tipo de merma inválido. Valores válidos: Danio, Defecto, Perdida.");

            // Obtener el ÚLTIMO costeo de ESTE COMPONENTE (no del lote específico)
            var ultimoCostingComponente = await _context.ComponentCostings
                .Include(cc => cc.ComponentLot)
                .Where(cc => cc.ComponentLot.ComponentId == lote.ComponentId)
                .OrderByDescending(cc => cc.Date)
                .FirstOrDefaultAsync();

            // Cálculo de valores basado en el historial del componente
            decimal promedioAnterior = ultimoCostingComponente?.Average ?? lote.UnitPrice;
            decimal saldoAnterior = ultimoCostingComponente?.Balance ?? (await GetTotalExistenciaComponente(lote.ComponentId)) * lote.UnitPrice;
            int existenciasAnteriores = ultimoCostingComponente?.Existence ?? await GetTotalExistenciaComponente(lote.ComponentId);

            // Validación de consistencia
            if (existenciasAnteriores != await GetTotalExistenciaComponente(lote.ComponentId))
            {
                return BadRequest($"Inconsistencia en existencias. Sistema: {existenciasAnteriores}, Real: {await GetTotalExistenciaComponente(lote.ComponentId)}");
            }

            // Cálculos finales
            decimal toHave = promedioAnterior * dto.Amount;
            int nuevasExistencias = existenciasAnteriores - dto.Amount;
            decimal saldoActualizado = saldoAnterior - toHave;

            // Actualización del lote específico
            lote.AvailableQuantity -= dto.Amount;

            // Registro de la merma
            var loss = new ComponentLoss
            {
                IdComponentLoss = Guid.NewGuid(),
                ComponentId = lote.ComponentId,
                Amount = dto.Amount,
                Date = DateTime.Now,
                Type = parsedType,
                Component = lote.Component
            };

            // Nuevo registro de costeo (específico por componente)
            var costing = new ComponentCosting
            {
                IdComponentCosting = Guid.NewGuid(),
                ComponentLotId = lote.IdComponentLot,
                ComponentLossId = loss.IdComponentLoss,
                Date = DateTime.Now,
                Entrance = 0,
                Exit = dto.Amount,
                Existence = nuevasExistencias,
                cost = lote.UnitPrice,
                Average = promedioAnterior, // Mantiene el promedio del componente
                Owes = 0,
                ToHave = toHave,
                Balance = saldoActualizado
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.ComponentLosses.Add(loss);
                _context.ComponentLots.Update(lote);
                _context.ComponentCostings.Add(costing);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "Merma registrada correctamente.",
                    Detalles = new
                    {
                        ComponenteId = lote.ComponentId,
                        LoteAfectado = lote.IdComponentLot,
                        ExistenciasComponenteAnteriores = existenciasAnteriores,
                        ExistenciasLoteAnteriores = lote.AvailableQuantity + dto.Amount,
                        NuevasExistenciasComponente = nuevasExistencias,
                        NuevasExistenciasLote = lote.AvailableQuantity,
                        PromedioUtilizado = promedioAnterior,
                        HaberCalculado = toHave,
                        SaldoAnterior = saldoAnterior,
                        NuevoSaldo = saldoActualizado
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error interno al procesar la merma: {ex.Message}");
            }
        }

        // Método auxiliar para obtener la existencia total de un componente
        private async Task<int> GetTotalExistenciaComponente(Guid componentId)
        {
            return await _context.ComponentLots
                .Where(cl => cl.ComponentId == componentId)
                .SumAsync(cl => cl.AvailableQuantity);
        }

        [HttpGet("GetComponentsLotWithAvailableQuantity")]
        public async Task<IActionResult> GetComponentsWithAvailableQuantity()
        {
            var componentesConCantidad = await _context.Components
                .Select(c => new
                {
                    c.IdComponent,
                    c.Name,
                    c.Description,
                    c.Category,
                    AvailableQuantity = c.ComponentLots.Sum(cl => (int?)cl.AvailableQuantity) ?? 0
                })
                .ToListAsync();

            return Ok(componentesConCantidad);
        }

        [HttpGet("GetComponentLotQuantities")]
        public async Task<IActionResult> GetComponentLotQuantities()
        {
            var lotes = await _context.ComponentLots
                .Include(cl => cl.Component)
                .Select(cl => new ComponentLotInfoDto
                {
                    ComponentLotId = cl.IdComponentLot,
                    ComponentName = cl.Component.Name,
                    TotalAmount = cl.Amount,
                    AvailableQuantity = cl.AvailableQuantity
                })
                .ToListAsync();

            return Ok(lotes);
        }


    }


}

