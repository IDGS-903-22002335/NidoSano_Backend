using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modulap.Data;
using Modulap.Models;

namespace Modulap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public DashboardController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }






        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _context.SaleDetails
    .Include(sd => sd.ChickenCoop)
    .Include(sd => sd.Sale)
    .Where(sd => sd.Sale.RegistrationDate >= startDate
              && sd.Sale.RegistrationDate <= endDate
              && sd.Sale.Type == TypeStatus.Entregado) // <-- filtro agregado
    .GroupBy(sd => sd.ChickenCoop.Name)
    .Select(g => new
    {
        ProductName = g.Key,
        QuantitySold = g.Sum(sd => sd.Amount),
        TotalRevenue = g.Sum(sd => sd.Amount * sd.UnitPrice)
    })
    .OrderByDescending(x => x.QuantitySold)
    .ToListAsync();


            return Ok(result);
        }

    }
}
