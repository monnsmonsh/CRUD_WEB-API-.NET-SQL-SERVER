using Menu.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

    }
}
