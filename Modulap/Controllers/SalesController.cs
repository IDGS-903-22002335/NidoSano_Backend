using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modulap.Data;
using Modulap.Dto;
using Modulap.Models;
using System.Security.Claims;

namespace Modulap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public SalesController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("GenerateEstimate")]
        public async Task<IActionResult> GenerateEstimate([FromBody] EstimateRequestDto dto)
        {
            decimal totalComponentsCost = 0m;

            var componentMap = new Dictionary<string, (string name, int multiplier)>
    {
        { "EnvironmentalMonitoring", ("Sensor DHT22", 1) },
        { "Airqualitymonitoring", ("Sensor MQ-135", 1) },
        { "Naturallightingmonitoring", ("LDR", 1) },
        { "Automaticfeeddispenser", ("Servomotor SG90", 1) },
        { "Waterlevelgauge", ("Sensor de nivel por flotador", 1) },
        { "NightMotionSensor", ("Sensor PIR HC‑SR501", 3) }
    };

            var defaultComponents = new List<(string name, int quantity)>
    {
        ("ESP32 DEVKIT", 2),
        ("Fuente de alimentación (5V + 12V)", 2),
        ("Protoboard", 2)
    };

            var selectedComponents = componentMap
                .Where(entry => dto.GetType().GetProperty(entry.Key)?.GetValue(dto)?.ToString()?.ToLower() == "si")
                .Select(entry => entry.Value.name)
                .ToList();

            if (dto.Automaticfeeddispenser?.ToLower() == "si")
            {
                selectedComponents.Add("Sensor ultrasónico HC-SR04");
            }

            selectedComponents.AddRange(defaultComponents.Select(c => c.name));

            selectedComponents = selectedComponents.Distinct().ToList();

            var components = await _context.Components
                .Where(c => selectedComponents.Contains(c.Name))
                .ToListAsync();

            var componentIds = components.Select(c => c.IdComponent).ToList();

            var latestCostsQuery = _context.ComponentCostings
                .Where(cc => componentIds.Contains(cc.ComponentLot.ComponentId))
                .GroupBy(cc => cc.ComponentLot.ComponentId)
                .Select(g => new
                {
                    ComponentId = g.Key,
                    LatestAverage = g.OrderByDescending(cc => cc.Date).Select(cc => cc.Average).FirstOrDefault()
                });

            var latestCosts = await latestCostsQuery.ToListAsync();

            foreach (var entry in componentMap)
            {
                var propValue = dto.GetType().GetProperty(entry.Key)?.GetValue(dto)?.ToString();
                if (propValue?.ToLower() == "si")
                {
                    var component = components.FirstOrDefault(c => c.Name == entry.Value.name);
                    if (component != null)
                    {
                        var cost = latestCosts.FirstOrDefault(lc => lc.ComponentId == component.IdComponent)?.LatestAverage ?? 0m;
                        totalComponentsCost += cost * entry.Value.multiplier;
                    }
                }
            }

            if (selectedComponents.Contains("Sensor ultrasónico HC-SR04"))
            {
                var ultrasonic = components.FirstOrDefault(c => c.Name == "Sensor ultrasónico HC-SR04");
                if (ultrasonic != null)
                {
                    var cost = latestCosts.FirstOrDefault(lc => lc.ComponentId == ultrasonic.IdComponent)?.LatestAverage ?? 0m;
                    totalComponentsCost += cost; 
                }
            }

           
            foreach (var item in defaultComponents)
            {
                var component = components.FirstOrDefault(c => c.Name == item.name);
                if (component != null)
                {
                    var cost = latestCosts.FirstOrDefault(lc => lc.ComponentId == component.IdComponent)?.LatestAverage ?? 0m;
                    totalComponentsCost += cost * item.quantity;
                }
            }

            
            decimal totalWithProfit = totalComponentsCost * 1.3m; 
            decimal chickensCost = dto.QuantityChickens * 50;
            decimal installationCost = dto.PhysicalInstallation?.ToLower() == "si" ? 400 : 0;
            decimal exteriorCost = dto.ChickenCoopLocation?.ToLower() == "exterior" ? 200 : 0;
            decimal totalPrice = totalWithProfit + chickensCost + installationCost + exteriorCost;

            var estimate = new Estimate
            {
                IdEstimate = Guid.NewGuid(),
                ClientId = dto.ClientId,
                CreationDate = DateTime.Now,
                ChickenCoopLocation = dto.ChickenCoopLocation,
                QuantityChickens = dto.QuantityChickens,
                EnvironmentalMonitoring = dto.EnvironmentalMonitoring,
                Airqualitymonitoring = dto.Airqualitymonitoring,
                Naturallightingmonitoring = dto.Naturallightingmonitoring,
                Automaticfeeddispenser = dto.Automaticfeeddispenser,
                Waterlevelgauge = dto.Waterlevelgauge,
                NightMotionSensor = dto.NightMotionSensor,
                connectiontype = dto.connectiontype,
                PhysicalInstallation = dto.PhysicalInstallation,
                PriceTotal = totalPrice
            };

            _context.Estimates.Add(estimate);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Cotización generada exitosamente",
                IdEstimate = estimate.IdEstimate,
                PrecioTotal = totalPrice,
                Desglose = new
                {
                    CostoComponentes = totalComponentsCost,
                    MargenGanancia = totalWithProfit - totalComponentsCost,
                    Gallinas = chickensCost,
                    Instalacion = installationCost,
                    Exterior = exteriorCost
                }
            });

        }




        [HttpPost("PreviewEstimate")]
        public async Task<IActionResult> PreviewEstimate([FromBody] EstimateRequestDto1 dto)
        {
            decimal totalComponentsCost = 0m;

            var componentMap = new Dictionary<string, (string name, int multiplier)>
    {
        { "EnvironmentalMonitoring", ("Sensor DHT22", 1) },
        { "Airqualitymonitoring", ("Sensor MQ-135", 1) },
        { "Naturallightingmonitoring", ("LDR", 1) },
        { "Automaticfeeddispenser", ("Servomotor SG90", 1) },
        { "Waterlevelgauge", ("Sensor de nivel por flotador", 1) },
        { "NightMotionSensor", ("Sensor PIR HC‑SR501", 3) }
    };

            var defaultComponents = new List<(string name, int quantity)>
    {
        ("ESP32 DEVKIT", 2),
        ("Fuente de alimentación (5V + 12V)", 2),
        ("Protoboard", 2)
    };

            var selectedComponents = componentMap
                .Where(entry => dto.GetType().GetProperty(entry.Key)?.GetValue(dto)?.ToString()?.ToLower() == "si")
                .Select(entry => entry.Value.name)
                .ToList();

            if (dto.Automaticfeeddispenser?.ToLower() == "si")
            {
                selectedComponents.Add("Sensor ultrasónico HC-SR04");
            }

            selectedComponents.AddRange(defaultComponents.Select(c => c.name));
            selectedComponents = selectedComponents.Distinct().ToList();

            var components = await _context.Components
                .Where(c => selectedComponents.Contains(c.Name))
                .ToListAsync();

            var componentIds = components.Select(c => c.IdComponent).ToList();

            var latestCostsQuery = _context.ComponentCostings
                .Where(cc => componentIds.Contains(cc.ComponentLot.ComponentId))
                .GroupBy(cc => cc.ComponentLot.ComponentId)
                .Select(g => new
                {
                    ComponentId = g.Key,
                    LatestAverage = g.OrderByDescending(cc => cc.Date).Select(cc => cc.Average).FirstOrDefault()
                });

            var latestCosts = await latestCostsQuery.ToListAsync();

            foreach (var entry in componentMap)
            {
                var propValue = dto.GetType().GetProperty(entry.Key)?.GetValue(dto)?.ToString();
                if (propValue?.ToLower() == "si")
                {
                    var component = components.FirstOrDefault(c => c.Name == entry.Value.name);
                    if (component != null)
                    {
                        var cost = latestCosts.FirstOrDefault(lc => lc.ComponentId == component.IdComponent)?.LatestAverage ?? 0m;
                        totalComponentsCost += cost * entry.Value.multiplier;
                    }
                }
            }

            if (selectedComponents.Contains("Sensor ultrasónico HC-SR04"))
            {
                var ultrasonic = components.FirstOrDefault(c => c.Name == "Sensor ultrasónico HC-SR04");
                if (ultrasonic != null)
                {
                    var cost = latestCosts.FirstOrDefault(lc => lc.ComponentId == ultrasonic.IdComponent)?.LatestAverage ?? 0m;
                    totalComponentsCost += cost;
                }
            }

            foreach (var item in defaultComponents)
            {
                var component = components.FirstOrDefault(c => c.Name == item.name);
                if (component != null)
                {
                    var cost = latestCosts.FirstOrDefault(lc => lc.ComponentId == component.IdComponent)?.LatestAverage ?? 0m;
                    totalComponentsCost += cost * item.quantity;
                }
            }

            decimal totalWithProfit = totalComponentsCost * 1.3m;
            decimal chickensCost = dto.QuantityChickens * 50;
            decimal installationCost = dto.PhysicalInstallation?.ToLower() == "si" ? 400 : 0;
            decimal exteriorCost = dto.ChickenCoopLocation?.ToLower() == "exterior" ? 200 : 0;
            decimal totalPrice = totalWithProfit + chickensCost + installationCost + exteriorCost;

            return Ok(new
            {
                Mensaje = "Vista previa generada exitosamente",
                PrecioTotal = totalPrice,
                Desglose = new
                {
                    CostoComponentes = totalComponentsCost,
                    MargenGanancia = totalWithProfit - totalComponentsCost,
                    Gallinas = chickensCost,
                    Instalacion = installationCost,
                    Exterior = exteriorCost
                }
            });
        }


        // cambio de estatus de Estimacion a Compra y registro de venta con estatus pendiente

        [HttpPost("RegisterSaleFromEstimate")]
        public async Task<IActionResult> RegisterSaleFromEstimate([FromBody] RegisterSaleFromEstimateDto dto)
        {
            if (dto == null || dto.EstimateId == Guid.Empty || dto.CantidadProductos <= 0)
            {
                return BadRequest("Debe proporcionar una estimación válida y una cantidad mayor a 0.");
            }

            
            var estimate = await _context.Estimates
                .FirstOrDefaultAsync(e => e.IdEstimate == dto.EstimateId && e.Status != Estado.Comprar);

            if (estimate == null)
            {
                return NotFound("No se encontró una estimación disponible.");
            }

            
            estimate.Status = Estado.Comprar;

            
            var sale = new Sale
            {
                IdSale = Guid.NewGuid(),
                UserId = estimate.ClientId,
                EstimateId = estimate.IdEstimate,
                RegistrationDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(10),
                Type = TypeStatus.Pendiente,
                TotalPrice = estimate.PriceTotal * dto.CantidadProductos
            };

            
            var saleDetail = new SaleDetail
            {
                IdSaleDetail = Guid.NewGuid(),
                SaleId = sale.IdSale,
                Amount = dto.CantidadProductos,
                UnitPrice = estimate.PriceTotal,
             
            };

            _context.Sales.Add(sale);
            _context.SaleDetails.Add(saleDetail);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Venta registrada correctamente a partir de la estimación.",
                Venta = new
                {
                    sale.IdSale,
                    sale.RegistrationDate,
                    sale.TotalPrice,
                    sale.Type,
                    Detalle = new
                    {
                        saleDetail.Amount,
                        saleDetail.UnitPrice,
                        saleDetail.ChickenCoopId
                    }
                }
            });
        }


        // Actualizacion de la venta a estatus a atendido y descuenta la cantidad de productos en inventario

        [HttpPut("UpdateSaleWithChickenCoop")]
        public async Task<IActionResult> UpdateSaleWithChickenCoop([FromBody] UpdateSaleDto dto)
        {
            if (dto == null || dto.SaleId == Guid.Empty || dto.ChickenCoopId == Guid.Empty)
            {
                return BadRequest("Se requiere SaleId y ChickenCoopId válidos.");
            }

            
            var sale = await _context.Sales
                .Include(s => s.SaleDetails)
                .FirstOrDefaultAsync(s => s.IdSale == dto.SaleId);

            if (sale == null)
                return NotFound("Venta no encontrada.");

            var saleDetail = sale.SaleDetails.FirstOrDefault();
            if (saleDetail == null)
                return BadRequest("La venta no tiene detalles asociados.");

            
            var chickenCoop = await _context.ChickenCoops
                .Include(c => c.Recipes)
                .ThenInclude(r => r.ProductionLots)
                .FirstOrDefaultAsync(c => c.IdChickenCoop == dto.ChickenCoopId);

            if (chickenCoop == null)
                return NotFound("Gallinero (ChickenCoop) no encontrado.");

            var recipe = chickenCoop.Recipes.FirstOrDefault();
            if (recipe == null)
                return BadRequest("El gallinero no tiene una receta asociada.");

            var lots = recipe.ProductionLots
                .Where(l => l.Status == Status.Terminado && l.AvailableQuantity > 0)
                .OrderBy(l => l.DateProduction)
                .ToList();

            int cantidadRestante = saleDetail.Amount;

            foreach (var lot in lots)
            {
                if (cantidadRestante <= 0)
                    break;

                if (lot.AvailableQuantity >= cantidadRestante)
                {
                    lot.AvailableQuantity -= cantidadRestante;
                    cantidadRestante = 0;
                }
                else
                {
                    cantidadRestante -= lot.AvailableQuantity;
                    lot.AvailableQuantity = 0;
                }
            }

            if (cantidadRestante > 0)
                return BadRequest("No hay suficiente inventario disponible en los lotes de producción.");

            sale.Type = TypeStatus.Atendida;

            saleDetail.ChickenCoopId = dto.ChickenCoopId;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Venta actualizada correctamente. Lotes descontados.",
                Venta = new
                {
                    sale.IdSale,
                    sale.Type,
                    ChickenCoopId = saleDetail.ChickenCoopId
                }
            });
        }



        // endPoint solo para cambiar el status de la venta
        [HttpPut("ChangeSaleStatus")]
        public async Task<IActionResult> ChangeSaleStatus([FromBody] UpdateSaleStatusDto dto)
        {
            if (dto == null || dto.SaleId == Guid.Empty)
            {
                return BadRequest("SaleId y nuevo estado son requeridos.");
            }

            var sale = await _context.Sales.FindAsync(dto.SaleId);

            if (sale == null)
                return NotFound("Venta no encontrada.");

            var estadosValidos = new[] { TypeStatus.Cancelado, TypeStatus.Enviado, TypeStatus.Entregado };
            if (!estadosValidos.Contains(dto.NewStatus))
            {
                return BadRequest("El estado enviado no es válido para este cambio.");
            }

            sale.Type = dto.NewStatus;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = $"El estado de la venta ha sido actualizado a {dto.NewStatus}.",
                Venta = new
                {
                    sale.IdSale,
                    sale.Type
                }
            });
        }

        [HttpGet("GetSalesWithDetails")]
        public async Task<IActionResult> GetSalesWithDetails()
        {
            var sales = await _context.Sales
                .Include(s => s.SaleDetails)
                .Include(s => s.Estimate)
                .Include(s => s.User)
                 // <-- Filtrar aquí las ventas con Type diferente de 0
                .OrderByDescending(s => s.RegistrationDate)
                .ToListAsync();

            var result = sales.Select(s => new
            {
                s.IdSale,
                s.UserId,
                ClienteNombre = s.User != null ? s.User.FullName : "Desconocido",
                s.RegistrationDate,
                s.DeliveryDate,
                s.Type,
                s.TotalPrice,
                Estimate = s.Estimate == null ? null : new
                {
                    s.Estimate.IdEstimate,
                    s.Estimate.ChickenCoopLocation,
                    s.Estimate.QuantityChickens,
                    s.Estimate.PriceTotal,
                    s.Estimate.EnvironmentalMonitoring,
                    s.Estimate.Airqualitymonitoring,
                    s.Estimate.Naturallightingmonitoring,
                    s.Estimate.Automaticfeeddispenser,
                    s.Estimate.Waterlevelgauge,
                    s.Estimate.NightMotionSensor,
                    s.Estimate.connectiontype,
                    s.Estimate.PhysicalInstallation
                },
                SaleDetails = s.SaleDetails.Select(d => new
                {
                    d.IdSaleDetail,
                    d.Amount,
                    d.UnitPrice,
                    d.ChickenCoopId
                }).ToList()
            });

            return Ok(result);
        }




        // Mostrar informacion Completa al cliente

        [Authorize]
        [HttpGet("GetPurchasedProducts")]
        public async Task<IActionResult> GetPurchasedProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var purchasedProducts = await _context.SaleDetails
                .Include(sd => sd.Sale)
                .Include(sd => sd.ChickenCoop)
                    .ThenInclude(cc => cc.Recipes)
                        .ThenInclude(r => r.RecipeDetail)
                            .ThenInclude(rd => rd.Component)
                .Where(sd => sd.Sale.UserId == userId)
                .Select(sd => new
                {
                    sd.IdSaleDetail,
                    SaleId = sd.SaleId,
                    CustomerId = sd.Sale.UserId,
                    SaleDate = sd.Sale.RegistrationDate,
                    Type = sd.Sale.Type, // <-- Añadido aquí
                    Quantity = sd.Amount,
                    UnitPrice = sd.UnitPrice,
                    Total = sd.UnitPrice * sd.Amount,
                    ChickenCoop = sd.ChickenCoop == null ? null : new
                    {
                        sd.ChickenCoop.IdChickenCoop,
                        sd.ChickenCoop.Name,
                        Recipes = sd.ChickenCoop.Recipes.Select(r => new
                        {
                            r.IdRecipe,
                            r.Description,
                            r.Amount,
                            RecipeDetails = r.RecipeDetail.Select(rd => new
                            {
                                rd.IdRecipeDetail,
                                rd.ComponentQuantity,
                                Component = new
                                {
                                    rd.Component.IdComponent,
                                    rd.Component.Name,
                                    rd.Component.Description
                                }
                            }).ToList()
                        }).ToList()
                    }
                })
                .OrderByDescending(p => p.SaleDate)
                .ToListAsync();

            return Ok(purchasedProducts);
        }



        [HttpGet("GetPurchasedProductsById/{saleId}")]
        public async Task<IActionResult> GetPurchasedProductsById(Guid saleId)
        {
            var purchasedProducts = await _context.SaleDetails
                .Where(sd => sd.SaleId == saleId)
                .Include(sd => sd.Sale)
                .Include(sd => sd.ChickenCoop)
                    .ThenInclude(cc => cc.Recipes)
                        .ThenInclude(r => r.RecipeDetail)
                            .ThenInclude(rd => rd.Component)
                .Select(sd => new
                {
                    sd.IdSaleDetail,
                    SaleId = sd.SaleId,
                    CustomerId = sd.Sale.UserId, // <-- ID del cliente
                    SaleDate = sd.Sale.RegistrationDate,
                    Type = sd.Sale.Type,
                    Quantity = sd.Amount,
                    UnitPrice = sd.UnitPrice,
                    Total = sd.UnitPrice * sd.Amount,
                    ChickenCoop = sd.ChickenCoop == null ? null : new
                    {
                        sd.ChickenCoop.IdChickenCoop,
                        sd.ChickenCoop.Name,
                        Recipes = sd.ChickenCoop.Recipes.Select(r => new
                        {
                            r.IdRecipe,
                            r.Description,
                            r.Amount,
                            RecipeDetails = r.RecipeDetail.Select(rd => new
                            {
                                rd.IdRecipeDetail,
                                rd.ComponentQuantity,
                                Component = new
                                {
                                    rd.Component.IdComponent,
                                    rd.Component.Name,
                                    rd.Component.Description
                                }
                            }).ToList()
                        }).ToList()
                    }
                })
                .OrderByDescending(p => p.SaleDate)
                .ToListAsync();

            if (purchasedProducts == null || !purchasedProducts.Any())
                return NotFound($"No se encontraron productos comprados para la venta con ID {saleId}");

            return Ok(purchasedProducts);
        }



        [HttpGet("GetSaleWithDetailsById/{id}")]
        public async Task<IActionResult> GetSaleWithDetailsById(Guid id)
        {
            var sale = await _context.Sales
                .Include(s => s.SaleDetails)
                .Include(s => s.Estimate)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.IdSale == id);

            if (sale == null)
            {
                return NotFound(new { message = "Venta no encontrada." });
            }

            var result = new
            {
                sale.IdSale,
                sale.UserId,
                ClienteNombre = sale.User != null ? sale.User.FullName : "Desconocido",
                sale.RegistrationDate,
                sale.DeliveryDate,
                sale.Type,
                sale.TotalPrice,
                Estimate = sale.Estimate == null ? null : new
                {
                    sale.Estimate.IdEstimate,
                    sale.Estimate.ChickenCoopLocation,
                    sale.Estimate.QuantityChickens,
                    sale.Estimate.PriceTotal,
                    sale.Estimate.EnvironmentalMonitoring,
                    sale.Estimate.Airqualitymonitoring,
                    sale.Estimate.Naturallightingmonitoring,
                    sale.Estimate.Automaticfeeddispenser,
                    sale.Estimate.Waterlevelgauge,
                    sale.Estimate.NightMotionSensor,
                    sale.Estimate.connectiontype,
                    sale.Estimate.PhysicalInstallation
                },
                SaleDetails = sale.SaleDetails.Select(d => new
                {
                    d.IdSaleDetail,
                    d.Amount,
                    d.UnitPrice,
                    d.ChickenCoopId
                }).ToList()
            };

            return Ok(result);
        }


    }
}
