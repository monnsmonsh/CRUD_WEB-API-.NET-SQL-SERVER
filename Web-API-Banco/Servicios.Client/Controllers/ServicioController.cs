using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Servicios.Client.Models;
using Servicios.Server.Models;
using System.Text;

namespace Servicios.Client.Controllers
{
    public class ServicioController : Controller
    {
        //inyectamos nuestro servicio HTTPCLIENT
        private readonly HttpClient _httpClient;
        public ServicioController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            //local host de Servicios.Server
            _httpClient.BaseAddress = new Uri("https://localhost:7275/api");
        }

        public async Task<IActionResult> Index()
        {
            //realizamos nuestra solicitud a nuestra WEB-API
            var response = await _httpClient.GetAsync("/api/Servicios/lista");

            //validamos la respuesta
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                //Creamos una var PRODUCTO para deserializar la respuesta
                var servicios = JsonConvert.DeserializeObject<IEnumerable<ServiciosViewModel>>(content);

                return View("Index", servicios);
            }

            //si no devuelve code 200
            //devolvemos una lsita vavcio
            return View(new List<ServiciosViewModel>());

        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(servicio);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Servicios/crear", content);

                //si nos devuelve un codigo 200
                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear nuevo servicio");
                }

            }
            return View(servicio);

        }




        //
        // Editar producto
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/Servicios/ver?id={id}");

            //validamos el codigo de respuesta
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var servicio = JsonConvert.DeserializeObject<ServiciosViewModel>(content);
                return View(servicio);
            }
            else//en caso de que no debuelva code200
            {
                return RedirectToAction("Details");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ServiciosViewModel servicio)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(servicio);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //ruta que se encuentra en el controlador
                var response = await _httpClient.PutAsync($"/api/Servicios/editar?id={id}", content);

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
            return View(servicio);
        }



        //
        // Monstrar un servicio en expecifico
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/Servicios/ver?id={id}");

            //validamos el codigo de respuesta
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var servicio = JsonConvert.DeserializeObject<ServiciosViewModel>(content);

                return View(servicio);

            }
            else
            {
                return RedirectToAction("Details");
            }
        }

        //
        // Metodo para eliminar
        public async Task<IActionResult>Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Servicios/eliminar?id={id}");
            //var response = await _httpClient.DeleteAsync($"/api/Servicios/eliminar?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Erro"] = "Error al eliminar Servicio";
                return RedirectToAction("Index");
            }

        }

    }
}
