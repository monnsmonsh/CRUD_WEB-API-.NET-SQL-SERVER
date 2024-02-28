using Menu.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Menu.Server.Controllers
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

        //Creamos metodos post, get, delete
        [HttpPost]
        [Route("crear")]
        //Metodo para guardar en BD
        public async Task<IActionResult> CrearProducto(Producto producto)
        {
            //guardamos en BD en la tblname, pasando el parametro
            await _context.Productos.AddAsync(producto);

            //guardamos cambios en BD
            await _context.SaveChangesAsync();

            //returnamos un mensaje de exito
            return Ok();

        }

        [HttpGet]
        [Route("lista")]
        //Metodo para listar
        public async Task<ActionResult<IEnumerable<Producto>>> ListarProductos()
        {

            //Obtenemos la lista de productos de la BD
            var productos = await _context.Productos.ToListAsync();

            //dvolvemos lista de productos
            return Ok(productos);

        }

        [HttpGet]
        [Route("ver")]
        //Metodo para consultar 
        public async Task<IActionResult> VerProducto(int id)
        {
            Producto producto = await _context.Productos.FindAsync(id);

            //validamos id
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPut]
        [Route("editar")]
        //Metodo para editar un producto
        public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
        {
            var productoExistente = await _context.Productos.FindAsync(id);

            //sustituimos los valores
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;

            //guardamos los cambios en la BD
            await _context.SaveChangesAsync();

            return Ok();

        }

        [HttpDelete]
        [Route("eliminar")]
        //Metodo para editar un producto
        public async Task<IActionResult> EliminarProducto(int id)
        {
            //Eliminar producto de la base de datos
            var productoEliminado = await _context.Productos.FindAsync(id);

            //aplicamos metod del parametro
            _context.Productos.Remove(productoEliminado!);

            await _context.SaveChangesAsync();
            return Ok();


        }

    }
}
