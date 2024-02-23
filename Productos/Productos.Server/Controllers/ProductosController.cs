using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Productos.Server.Models;

namespace Productos.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        //inyectamos contexto de BD 
        private readonly ProductosContext _context;
        public ProductosController(ProductosContext context)
        {
            _context = context;
        }


        //Creamos metodos
        [HttpPost]
        [Route("crear")]
        public async Task<IActionResult>CrearProducto(Producto producto)
        {
            //guardamos en BD pasando el parametro
            await _context.Productos.AddAsync(producto);
            
            //guardamos cambios en BD
            await _context.SaveChangesAsync();

            //returnamos
            return Ok();
        
        }

    }
}
