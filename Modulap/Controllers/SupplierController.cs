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
    public class SupplierController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public SupplierController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        //Guardar

        [HttpPost("registerSupplier")]
        public async Task<ActionResult<string>> Register(RegistesSupplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } // validacion auntomatica para los datos del cuerpo de la solicitud

            var proveedor = new Supplier  // creo una instacia  de supplier y le asigno desde el dto llamado  RegistesSupplier
            {
                IdSupplier = supplier.IdSupplier,
                Name = supplier.Name,
                PhoneNumber = supplier.PhoneNumber,
                Email = supplier.Email,
                Address = supplier.Address,
                RegistrationDate = supplier.RegistrationDate,
                status = supplier.status
            };

            await _context.Suppliers.AddAsync(proveedor);  // uso un contexto de basee de datos para agregar la nueva entidad proveedor que es un objeto de tipo supplier al conjunto suppliers por medio de Addansyc  que marca a proveedor para que se guarde a la base de datos
            await _context.SaveChangesAsync(); // guarda los cambios a la base de datos, es la llamada que ejecuta insert para guardar el nuevo registro

            return Ok(new { message = "Proveedor registrado exitosamente", proveedor.IdSupplier });
        }

        // eliminar

        [HttpPut("deleteSupplier")]
        public async Task<IActionResult> DeleteSupplier([FromBody] DeleteSupplierDto dto)
        {
            var supplier = await _context.Suppliers.FindAsync(dto.IdSupplier);

            if (supplier == null)
            {
                return NotFound(new { message = "Proveedor no encontrado" });
            }

            supplier.status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Proveedor eliminado", supplier.IdSupplier });
        }

        // actualizar datos
        [HttpPut("updateSupplier")]
        public async Task<IActionResult> UpdateSupplier([FromBody] UpdateSupplierDto dto)
        {
            var supplier = await _context.Suppliers.FindAsync(dto.IdSupplier);

            if (supplier == null)
            {
                return NotFound(new { message = "Proveedor no encontrado" });
            }


            supplier.Name = dto.Name;
            supplier.PhoneNumber = dto.PhoneNumber;
            supplier.Email = dto.Email;
            supplier.Address = dto.Address;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Proveedor actualizado correctamente", supplier.IdSupplier });
        }

        // ver detalles
        [HttpGet("getSupplierById/{id}")]
        public async Task<IActionResult> GetSupplierById(Guid id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound(new { message = "Proveedor no encontrado" });
            }

            return Ok(supplier);
        }




        [HttpGet("getActiveSuppliers")]
        public async Task<IActionResult> GetActiveSuppliers()
        {
            var suppliers = await _context.Suppliers
                .Where(s => s.status == 1)
                .ToListAsync();

            return Ok(suppliers);
        }

    }

    }
