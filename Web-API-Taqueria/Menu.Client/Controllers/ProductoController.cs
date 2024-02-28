using Menu.Client.Models;
using Menu.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Menu.Client.Controllers
{
    public class ProductoController : Controller
    {
        //inyectamos nuestro servicio HTTPCLIENT
        private readonly HttpClient _httpClient;
        public ProductoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            //local host de Productos.Server
            _httpClient.BaseAddress = new Uri("https://localhost:7175/api");
        }

        public async Task<IActionResult> Index()
        {
            //realizamos nuestra solicitud a nuestra WEB-API
            var response = await _httpClient.GetAsync("/api/Productos/lista");

            //validamos la respuesta
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                //Creamos una var PRODUCTO para deserializar la respuesta
                var productos = JsonConvert.DeserializeObject<IEnumerable<ProductoViewModel>>(content);

                return View("Index", productos);
            }

            //si no devuelve code 200
            //devolvemos una lsita vavcio
            return View(new List<ProductoViewModel>());

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Productos/crear", content);

                //si nos devuelve un codigo 200
                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear producto");
                }

            }
            return View(producto);

        }

        //
        /// Editar producto
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/Productos/ver?id={id}");

            //validamos el codigo de respuesta
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var producto = JsonConvert.DeserializeObject<ProductoViewModel>(content);
                return View(producto);
            }
            else//en caso de que no debuelva code200
            {
                return RedirectToAction("Details");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductoViewModel producto)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //ruta que se encuentra en el controlador
                var response = await _httpClient.PutAsync($"/api/Productos/editar?id={id}", content);

                //validamos la respuesta
                if (response.IsSuccessStatusCode)
                {
                    // Manejamos la ctualización exitosa, redirigiendonos a la página de detalles del producto.
                    return RedirectToAction("Index", new { id });

                }
                else
                {
                    // Manejamos el error en la solicitud PUT o POST, mostrando un mensaje de error.
                    ModelState.AddModelError(string.Empty, "Error al actualizar el producto.");

                }

            }

            // Si hay errores de validación, volvemos a mostrar el formulario de edición con los errores.
            return View(producto);
        }

        //
        // Monstrar un producto en expecifico
        public async Task<IActionResult>Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/Productos/ver?id={id}");

            //validamos el codigo de respuesta
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var producto = JsonConvert.DeserializeObject<ProductoViewModel>(content);

                return View(producto);

            }
            else
            {
                return RedirectToAction("Details");
            }
        }

        //
        // Metodo para eliminar
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Productos/eliminar?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar Producto";
                return RedirectToAction("Details");
            }

        }





    }
}
