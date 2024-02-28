using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicios.Server.Models;

namespace Servicios.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {

        //inyectamos contexto de BD 
        private readonly ServiciosContext _context;
        public ServiciosController(ServiciosContext context)
        {
            _context = context;
        }


        //Creamos metodos post, get, delete
        [HttpPost]
        [Route("crear")]
        //Metodo para guardar en BD
        public async Task<IActionResult> CrearServicio(Servicio servicio)
        {
            //guardamos en BD en la tblname, pasando el parametro
            await _context.Servicios.AddAsync(servicio);

            //guardamos cambios en BD
            await _context.SaveChangesAsync();

            //returnamos un mensaje de exito
            return Ok();

        }


        [HttpGet]
        [Route("lista")]
        //Metodo para listar
        public async Task<ActionResult<IEnumerable<Servicio>>> ListarServicios()
        {

            //Obtenemos la lista de productos de la BD
            var productos = await _context.Servicios.ToListAsync();

            //dvolvemos lista de productos
            return Ok(productos);

        }


        [HttpGet]
        [Route("ver")]
        //Metodo para consultar 
        public async Task<IActionResult> VerProducto(int id)
        {
            Servicio servicio = await _context.Servicios.FindAsync(id);

            //validamos id
            if (servicio == null)
            {
                return NotFound();
            }
            return Ok(servicio);
        }


        [HttpPut]
        [Route("editar")]
        //Metodo para editar un producto
        public async Task<IActionResult> ActualizarServicio(int id, Servicio servicio)
        {
            var servicioExistente = await _context.Servicios.FindAsync(id);

            //sustituimos los valores
            servicioExistente.Nombre = servicio.Nombre;
            servicioExistente.Descripcion = servicio.Descripcion;
            servicioExistente.TipoServicio = servicio.TipoServicio;
            servicioExistente.Precio = servicio.Precio;

            //guardamos los cambios en la BD
            await _context.SaveChangesAsync();

            return Ok();

        }


        [HttpDelete]
        [Route("eliminar")]
        //Metodo para editar un producto
        public async Task<IActionResult> EliminarServicio(int id)
        {
            var servicioEliminado = await _context.Servicios.FindAsync(id);

            //aplicamos metod del parametro
            _context.Servicios.Remove(servicioEliminado!);

            await _context.SaveChangesAsync();
            return Ok();

        }


    }
}
