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
    public class BuysController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public BuysController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("BuysRegister")]
        public async Task<ActionResult<string>> BuysRegister([FromBody] BuysCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.ComponentLots.GroupBy(cl => cl.ComponentId).Any(g => g.Count() > 1))
                return BadRequest("No se permiten múltiples lotes del mismo componente en una compra");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Registrar la compra
                var newBuy = new Buys
                {
                    IdBuys = Guid.NewGuid(),
                    AdminId = dto.AdminId,
                    DateBuys = DateTime.Now,
                    Total = dto.ComponentLots.Sum(l => Math.Round(l.UnitPrice * l.Amount, 2))
                };
                _context.Buys.Add(newBuy);

                // 2. Procesar cada lote y recoger ChickenCoops afectados
                var affectedCoops = new HashSet<Guid>();
                var componentAverages = new Dictionary<Guid, decimal>(); // Guarda los nuevos promedios

                foreach (var lotDto in dto.ComponentLots)
                {
                    // Registrar el lote
                    var componentLot = new ComponentLot
                    {
                        IdComponentLot = Guid.NewGuid(),
                        ComponentId = lotDto.ComponentId,
                        SupplierId = lotDto.SupplierId,
                        BuysId = newBuy.IdBuys,
                        UnitPrice = lotDto.UnitPrice,
                        Amount = lotDto.Amount,
                        AvailableQuantity = lotDto.Amount
                    };
                    _context.ComponentLots.Add(componentLot);

                    // Registrar costeo y guardar el nuevo promedio
                    var newAverage = await RegisterComponentCosting(componentLot);
                    componentAverages[componentLot.ComponentId] = newAverage;

                    // Obtener ChickenCoops que usan este componente
                    var coopsUsingComponent = await _context.RecipeDetails
                        .Where(rd => rd.ComponentId == componentLot.ComponentId)
                        .Select(rd => rd.Recipe.ChickenCoopId)
                        .Distinct()
                        .ToListAsync();

                    foreach (var coopId in coopsUsingComponent)
                    {
                        affectedCoops.Add(coopId);
                    }
                }

                // 3. Actualizar precios de ChickenCoops afectados con los nuevos promedios
                if (affectedCoops.Any())
                {
                    await UpdateChickenCoopPrices(affectedCoops, componentAverages);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "Compra registrada correctamente",
                    UpdatedChickenCoops = affectedCoops.Count,
                    NewAverages = componentAverages
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error al registrar compra: {ex.Message}");
            }
        }

        private async Task<decimal> RegisterComponentCosting(ComponentLot componentLot)
        {
            // Obtener último costeo del componente
            var lastCosting = await _context.ComponentCostings
                .Include(cc => cc.ComponentLot)
                .Where(cc => cc.ComponentLot.ComponentId == componentLot.ComponentId)
                .OrderByDescending(cc => cc.Date)
                .FirstOrDefaultAsync();

            // Calcular nuevos valores
            int previousExistence = lastCosting?.Existence ?? 0;
            decimal previousBalance = lastCosting?.Balance ?? 0;
            decimal previousAverage = lastCosting?.Average ?? componentLot.UnitPrice;

            int newExistence = previousExistence + componentLot.Amount;
            decimal newDebt = Math.Round(componentLot.UnitPrice * componentLot.Amount, 2);
            decimal newBalance = Math.Round(previousBalance + newDebt, 2);
            decimal newAverage = newExistence > 0 ? Math.Round(newBalance / newExistence, 4) : 0;

            // Registrar nuevo costeo
            var costing = new ComponentCosting
            {
                IdComponentCosting = Guid.NewGuid(),
                ComponentLotId = componentLot.IdComponentLot,
                Date = DateTime.Now,
                Entrance = componentLot.Amount,
                Exit = 0,
                Existence = newExistence,
                cost = componentLot.UnitPrice,
                Average = newAverage,
                Owes = newDebt,
                ToHave = 0,
                Balance = newBalance
            };
            _context.ComponentCostings.Add(costing);

            return newAverage;
        }

        private async Task UpdateChickenCoopPrices(HashSet<Guid> chickenCoopIds, Dictionary<Guid, decimal> newComponentAverages)
        {
            // 1. Obtener todas las recetas y sus detalles para los ChickenCoops afectados
            var recipesWithDetails = await _context.Recipes
                .Include(r => r.RecipeDetail)
                .Include(r => r.ChickenCoop)
                .Where(r => chickenCoopIds.Contains(r.ChickenCoopId))
                .ToListAsync();

            // 2. Obtener los últimos promedios para todos los componentes necesarios
            var allComponentIds = recipesWithDetails
                .SelectMany(r => r.RecipeDetail.Select(rd => rd.ComponentId))
                .Distinct()
                .ToList();

            var currentAverages = await _context.ComponentCostings
                .Include(cc => cc.ComponentLot)
                .Where(cc => allComponentIds.Contains(cc.ComponentLot.ComponentId))
                .GroupBy(cc => cc.ComponentLot.ComponentId)
                .Select(g => new
                {
                    ComponentId = g.Key,
                    Average = g.OrderByDescending(cc => cc.Date).First().Average
                })
                .ToDictionaryAsync(x => x.ComponentId, x => x.Average);

            // 3. Combinar promedios: usar los nuevos para componentes actualizados, y los existentes para los demás
            foreach (var componentId in newComponentAverages.Keys)
            {
                currentAverages[componentId] = newComponentAverages[componentId];
            }

            // 4. Calcular y actualizar precios
            foreach (var recipe in recipesWithDetails)
            {
                decimal totalCost = 0m;

                foreach (var detail in recipe.RecipeDetail)
                {
                    if (currentAverages.TryGetValue(detail.ComponentId, out var average))
                    {
                        totalCost += Math.Round(average * detail.ComponentQuantity, 2);
                    }
                }

                // Aplicar margen del 30% y actualizar precio
                recipe.ChickenCoop.Price = Math.Round(totalCost * 1.30m, 2);
            }

            await _context.SaveChangesAsync();
        }
        // get de compras
        [HttpGet("GetAllWithDetails")]
        public async Task<ActionResult<IEnumerable<BuysDetailDto>>> GetAllWithDetails()
        {
            var compras = await _context.Buys
                .Include(b => b.Administrator)
                .Include(b => b.ComponentLots)
                    .ThenInclude(cl => cl.Component)
                .Include(b => b.ComponentLots)
                    .ThenInclude(cl => cl.Supplier)
                .ToListAsync();

            var result = compras.Select(buy => new BuysDetailDto
            {
                IdBuys = buy.IdBuys,
                AdminId = buy.AdminId,
                AdminFullName = buy.Administrator?.FullName,
                DateBuys = buy.DateBuys,
                Total = buy.Total,
                ComponentLots = buy.ComponentLots.Select(lot => new ComponentLotDetailDto
                {
                    IdComponentLot = lot.IdComponentLot,
                    ComponentId = lot.ComponentId,
                    ComponentName = lot.Component?.Name,
                    SupplierId = lot.SupplierId,
                    SupplierName = lot.Supplier?.Name,
                    UnitPrice = lot.UnitPrice,
                    Amount = lot.Amount,
                    AvailableQuantity = lot.AvailableQuantity
                }).ToList()
            }).ToList();

            return Ok(result);
        }


    }
}
