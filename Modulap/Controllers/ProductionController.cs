using Microsoft.AspNetCore.Mvc;
using Modulap.Data;
using Modulap.Dto;
using Modulap.Models;
using Microsoft.EntityFrameworkCore;

namespace Modulap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductionController(AppDbContext context)
        {
            _context = context;
        }


        //Para solicitar lote de productos
        [HttpPost("RegisterMultiple")]
        public async Task<IActionResult> RegisterMultipleProductionLots([FromBody] List<ProductionLotCreateDto> lotesDto)
        {
            if (lotesDto == null || !lotesDto.Any())
                return BadRequest("Se requiere al menos un lote de producción.");

            var newLots = new List<ProductionLot>();

            foreach (var dto in lotesDto)
            {
                var recipe = await _context.Recipes
                    .Include(r => r.RecipeDetail)
                    .ThenInclude(rd => rd.Component)
                    .FirstOrDefaultAsync(r => r.IdRecipe == dto.RecipeId);

                if (recipe == null)
                    return NotFound($"Receta no encontrada para RecipeId {dto.RecipeId}.");

                if (recipe.Amount <= 0)
                    return BadRequest($"La cantidad (Amount) en la receta con RecipeId {dto.RecipeId} debe ser mayor a 0.");

                var newProductionLot = new ProductionLot
                {
                    idProductionLot = Guid.NewGuid(),
                    RecipeId = dto.RecipeId,
                    DateProduction = dto.DateProduction,
                    AvailableQuantity = recipe.Amount,
                    Status = Status.Pendiente
                };

                newLots.Add(newProductionLot);
            }

            await _context.ProductionLots.AddRangeAsync(newLots);
            await _context.SaveChangesAsync();

            return Ok(new {Message = $"{newLots.Count} lotes de producción registrados con estado pendiente." });
        }


        [HttpPut("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] string newStatus)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var production = await _context.ProductionLots
                    .Include(p => p.Recipe)
                        .ThenInclude(r => r.RecipeDetail)
                            .ThenInclude(rd => rd.Component)
                    .FirstOrDefaultAsync(p => p.idProductionLot == id);

                if (production == null)
                    return NotFound("Lote de producción no encontrado.");

                if (!Enum.TryParse<Status>(newStatus, true, out var parsedStatus))
                    return BadRequest("Estado no válido.");

                if (production.Status == parsedStatus)
                    return BadRequest("El lote ya tiene este estado.");

                if (parsedStatus == Status.Produciendo)
                {
                    if (production.Recipe == null)
                        return BadRequest("La receta asociada no existe");

                    foreach (var detail in production.Recipe.RecipeDetail)
                    {
                        int quantityNeeded = detail.ComponentQuantity * production.AvailableQuantity;

                        var lots = await _context.ComponentLots
                            .Where(cl => cl.ComponentId == detail.ComponentId && cl.AvailableQuantity > 0)
                            .OrderBy(cl => cl.IdComponentLot)
                            .ToListAsync();

                        foreach (var lot in lots)
                        {
                            if (quantityNeeded == 0) break;

                            int quantityToUse = Math.Min(quantityNeeded, lot.AvailableQuantity);
                            lot.AvailableQuantity -= quantityToUse;
                            quantityNeeded -= quantityToUse;

                            var componentProduction = new ComponentProduction
                            {
                                IdComponentProduction = Guid.NewGuid(),
                                ComponentLotId = lot.IdComponentLot,
                                ProductionLotId = production.idProductionLot,
                                QuantityUsed = quantityToUse,
                                Date = DateTime.Now
                            };

                            await _context.ComponentProductions.AddAsync(componentProduction);
                            await _context.SaveChangesAsync(); 

                            await RegisterComponentCosting(
                                detail.ComponentId,
                                quantityToUse,
                                lot.UnitPrice,
                                componentProduction.IdComponentProduction,
                                lot.IdComponentLot 
                            );
                        }

                        if (quantityNeeded > 0)
                            return BadRequest($"No fue posible consumir la cantidad necesaria del componente: {detail.Component.Name}");
                    }
                }

                production.Status = parsedStatus;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = $"Estado actualizado a: {parsedStatus}",
                    ProductionLotId = production.idProductionLot
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error al cambiar estado: {ex.Message}");
            }
        }

        // 👇 Método actualizado con nuevo parámetro componentLotId
        private async Task RegisterComponentCosting(
            Guid componentId,
            int quantityUsed,
            decimal unitPrice,
            Guid componentProductionId,
            Guid componentLotId // 👈 nuevo parámetro
        )
        {
            var lastCosting = await _context.ComponentCostings
                .Include(cc => cc.ComponentLot)
                .Where(cc => cc.ComponentLot.ComponentId == componentId)
                .OrderByDescending(cc => cc.Date)
                .FirstOrDefaultAsync();

            int currentExistence = await _context.ComponentLots
                .Where(cl => cl.ComponentId == componentId)
                .SumAsync(cl => cl.AvailableQuantity);

            decimal previousAverage = lastCosting?.Average ?? unitPrice;
            int previousExistence = lastCosting?.Existence ?? currentExistence + quantityUsed;
            decimal previousBalance = lastCosting?.Balance ?? previousExistence * unitPrice;

            decimal haber = quantityUsed * previousAverage;
            int newExistence = previousExistence - quantityUsed;
            decimal newBalance = previousBalance - haber;

            var costing = new ComponentCosting
            {
                IdComponentCosting = Guid.NewGuid(),
                ComponentLotId = componentLotId, 
                ComponentProductionId = componentProductionId,
                Date = DateTime.Now,
                Entrance = 0,
                Exit = quantityUsed,
                Existence = newExistence,
                cost = unitPrice,
                Average = previousAverage,
                Owes = 0,
                ToHave = haber,
                Balance = newBalance
            };

            await _context.ComponentCostings.AddAsync(costing);
            await _context.SaveChangesAsync();
        }


        [HttpGet("GetAllAvailableProducts")]
        public async Task<IActionResult> GetAllAvailableProducts()
        {
            var data = await _context.Recipes
                .Include(r => r.ChickenCoop)
                .Select(r => new
                {
                    r.IdRecipe,
                    ProductName = r.ChickenCoop.Name,
                    TotalAvailableQuantity = _context.ProductionLots
                        .Where(pl => pl.RecipeId == r.IdRecipe && pl.Status == Status.Terminado)
                        .Sum(pl => (int?)pl.AvailableQuantity) ?? 0,  // suma o 0 si no hay registros
                })
                .ToListAsync();

            return Ok(data);
        }


        [HttpGet("GetAllproductRequested")]
        public async Task<IActionResult> GetAllproductRequested()
        {
            var data = await _context.Recipes
            .Include(r => r.ChickenCoop)
            .Select(r => new
            {
                r.IdRecipe,
                ProductName = r.ChickenCoop.Name,
                ProductionLots = _context.ProductionLots
                    .Where(pl => pl.RecipeId == r.IdRecipe
                                 && pl.Status != Status.Terminado
                                 && pl.Status != Status.Cancelado)
                    .Select(pl => new
                    {
                        pl.idProductionLot,
                        pl.DateProduction,
                        pl.AvailableQuantity,
                        Status = pl.Status.ToString() // 👈 aquí convertimos el enum a texto
                    })
                    .ToList()
            })
            .ToListAsync();

            return Ok(data);
        }






    }
}
