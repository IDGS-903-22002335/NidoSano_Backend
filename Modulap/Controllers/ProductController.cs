using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modulap.Data;
using Modulap.Dto;
using Modulap.Models;

namespace Modulap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;



        }


        [HttpPost("CreateChickenCoopWithRecipe")]
        public async Task<IActionResult> CreateChickenCoopWithRecipe([FromBody] ChickenCoopCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Crear el ChickenCoop
                var chickenCoop = new ChickenCoop
                {
                    IdChickenCoop = dto.IdChickenCoop,
                    Name = dto.Name,
                    Description = dto.Description,
                    Recipes = new List<Recipe>(),
                    AvailabilityStatus = Availability.Suficiente
                };

                decimal totalCost = 0m;

                foreach (var recipeDto in dto.Recipes)
                {
                    var recipe = new Recipe
                    {
                        IdRecipe = recipeDto.IdRecipe,
                        ChickenCoopId = chickenCoop.IdChickenCoop,
                        Amount = recipeDto.Amount,
                        Description = recipeDto.Description,
                        RecipeDetail = new List<RecipeDetail>()
                    };

                    foreach (var detailDto in recipeDto.Details)
                    {
                        // Obtener el ÚLTIMO costeo del componente (no del lote)
                        var lastCosting = await _context.ComponentCostings
                            .Include(cc => cc.ComponentLot)
                            .Where(cc => cc.ComponentLot.ComponentId == detailDto.ComponentId)
                            .OrderByDescending(cc => cc.Date)
                            .FirstOrDefaultAsync();

                        decimal componentAverage = lastCosting?.Average ??
                            await _context.ComponentLots
                                .Where(cl => cl.ComponentId == detailDto.ComponentId)
                                .OrderByDescending(cl => cl.IdComponentLot)
                                .Select(cl => cl.UnitPrice)
                                .FirstOrDefaultAsync();

                        if (componentAverage == 0)
                            return BadRequest($"No se encontró precio para el componente {detailDto.ComponentId}");

                        // Calcular con redondeo a 2 decimales
                        decimal componentCost = Math.Round(componentAverage * detailDto.ComponentQuantity, 2);
                        totalCost += componentCost;

                        var detail = new RecipeDetail
                        {
                            IdRecipeDetail = Guid.NewGuid(),
                            RecipeId = recipe.IdRecipe,
                            ComponentId = detailDto.ComponentId,
                            ComponentQuantity = detailDto.ComponentQuantity
                        };

                        recipe.RecipeDetail.Add(detail);
                    }

                    chickenCoop.Recipes.Add(recipe);
                }

                // Aplicar margen del 30% con redondeo
                chickenCoop.Price = Math.Round(totalCost * 1.30m, 2);

                _context.ChickenCoops.Add(chickenCoop);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "Receta guardada exitosamente.",
                    CostoTotal = totalCost,
                    PrecioFinal = chickenCoop.Price,
                    Margen = "30%"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error al crear producto: {ex.Message}");
            }
        }



        // solo hace otra variante del producto
        // solo crea la receta y sus detalles
        [HttpPost("AddRecipeToChickenCoop")]
        public async Task<IActionResult> AddRecipeToChickenCoop([FromBody] RecipeCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar que el ChickenCoop existe
            var chickenCoop = await _context.ChickenCoops.FindAsync(dto.ChickenCoopId);
            if (chickenCoop == null)
                return NotFound(new { message = "ChickenCoop no encontrado." });

            // Crear la nueva receta
            var recipe = new Recipe
            {
                IdRecipe = dto.IdRecipe,
                ChickenCoopId = dto.ChickenCoopId,
                Amount = dto.Amount,
                Description = dto.Description,
                RecipeDetail = new List<RecipeDetail>()
            };

            // Agregar los detalles
            foreach (var detailDto in dto.Details)
            {
                var detail = new RecipeDetail
                {
                    IdRecipeDetail = detailDto.IdRecipeDetail,
                    RecipeId = recipe.IdRecipe,
                    ComponentId = detailDto.ComponentId,
                    ComponentQuantity = detailDto.ComponentQuantity
                };

                recipe.RecipeDetail.Add(detail);
            }

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Receta agregada al ChickenCoop exitosamente." });
        }
        [HttpGet("GetChickenCoopWithRecipes")]
        public async Task<IActionResult> GetChickenCoopWithRecipes()
        {
            var chickenCoops = await _context.ChickenCoops
                .Include(cc => cc.Recipes)
                    .ThenInclude(r => r.RecipeDetail)
                        .ThenInclude(rd => rd.Component)
                .Select(cc => new ChickenCoopWithRecipesDto
                {
                    IdChickenCoop = cc.IdChickenCoop,
                    Name = cc.Name,
                    Description = cc.Description,
                    Price = cc.Price,
                    AvailabilityStatus = cc.AvailabilityStatus.ToString(),
                    Recipes = cc.Recipes.Select(r => new RecipeDto
                    {
                        IdRecipe = r.IdRecipe,
                        Description = r.Description,
                        Amount = r.Amount,
                        RecipeDetail = r.RecipeDetail.Select(rd => new RecipeDetailDto
                        {
                            IdRecipeDetail = rd.IdRecipeDetail,
                            ComponentQuantity = rd.ComponentQuantity,
                            Component = new ComponentDto
                            {
                                IdComponent = rd.Component.IdComponent,
                                Name = rd.Component.Name,
                                Description = rd.Component.Description,
                                Category = rd.Component.Category
                            }
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();

            return Ok(chickenCoops);
        }

        [HttpGet("GetAllBasic")]
        public async Task<IActionResult> GetAllBasic()
        {
            var chickenCoops = await _context.ChickenCoops
                .Select(cc => new ChickenCoopWithRecipesDto
                {
                    IdChickenCoop = cc.IdChickenCoop,
                    Name = cc.Name,
                    Description = cc.Description,
                    Price = cc.Price,
                    AvailabilityStatus = cc.AvailabilityStatus.ToString()
                })
                .ToListAsync();

            return Ok(chickenCoops);
        }


    }
}
