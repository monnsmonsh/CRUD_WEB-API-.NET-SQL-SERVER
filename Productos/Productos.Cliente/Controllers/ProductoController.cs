using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Productos.Cliente.Models;
using Productos.Server.Models;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace Productos.Cliente.Controllers
{
    public class ProductoController : Controller
    {
        //inyectamos nuestro servicio HTTPCLIENT
        private readonly HttpClient _httpClient;
        public ProductoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            //local host de Productos.Server
            _httpClient.BaseAddress = new Uri("https://localhost:7053/api");
        }

        
        public async Task<IActionResult> Inicio()
        {
            //realizamos nuestra solicitud a nuestra WEB-API
            var response = await _httpClient.GetAsync("/api/Productos/lista");

            //validamos la respuesta
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                //Creamos una var PRODUCTO para deserializar la respuesta
                var productos = JsonConvert.DeserializeObject<IEnumerable<ProductoViewModel>>(content);
           
                return View("Inicio", productos);
            }
            
            //si no devuelve code 200
            //devolvemos una lsita vavcio
            return View(new List<Producto>());

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            if(ModelState.IsValid)
            {
                var json =JsonConvert.SerializeObject(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Productos/crear", content);

                //si nos devuelve un codigo 200
                if (response.IsSuccessStatusCode) {

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear producto");
                }

            }
            return View(producto);

        }

    }
}
