# Crear WEB´S API con .NET y SQL SERVER

## Creacion de una Web api con asp.net

## Creacion de la API
### Inicializacion de un Proyecto

- Para inicializar un proyecto en Visual Studio lo primero que tenemos que realizar es dirigimos a `Crear un Proyecto`.
    <img src="/assets/img/01.png" width="80%">

- Posteriormente seleccionamos  el tipo de proyecto que necesitemos en este caso seleccionamos una `Solucion en Blanco` y le damos en `siguiente`.
    <img src="/assets/img/02.png" width="80%">

- Llegando hasta este paso definimos el nombre del proyecto y le damos en `Crear`.
    <img src="/assets/img/03.png" width="80%">

- Para empezar a crear nuestra API damos click derecho sobre nuestra soluccion nos deslizamos hasta la pestaña `Agregar` y seleccionamos  `Nuevo proyecto`.
    <img src="/assets/img/04.png" width="80%">

    - Seleccionamos el tipo de proyecto  `ASP.NET Core Web API C#` y le damos click en `Siguiente`
        <img src="/assets/img/05.png" width="80%">

    - Definimos el nombre del Proyecto de nuestra  que sera el servidor de nuestra  Web API le damos click en `Siguiente`
        <img src="/assets/img/06.png" width="80%">

    - Seleccionamos la configuracion deseada en este caso el Framework sera `.NET 8.0` sin Authentication de campo y con la configuracion que viene por defaul, para poder crear la soucion le damos click en `Crear`
        <img src="/assets/img/07.png" width="80%">

- Para poder trabajar con `SQL SERVER` necesitamos instalar algunos packetes para esto damos click derecho en nuestro proyecto, y nos deslizamos hasta donde este `Administrar paquetes NuGet para la solucion...`.
    <img src="/assets/img/08.png" width="80%">

    - Seleccionamos e instalamos los siguientes packetes para nuestro proyecto
    <img src="/assets/img/09.png" width="80%">

- Listo todo esto podemos empezar a crear nuestro proyecto lo primero que tenemos que hacer es crear una carpeta con el nombre `Models` para esto damos click derecho sobre nuestro proyecto nos desplazamos hasta agregar y seleccionamos nueva carpeta
    <img src="/assets/img/10.png" width="80%">

    - En la carpeta `Models` creamos una clase que no representara nuestra entidad para esto sobre la carpeta damos click derecho y `agregar` y seleccionamos `Clase...`
    <img src="/assets/img/11.png" width="80%">

    - Definimos los atrubutos de nuestra clase
    ```C#
    public class NameClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal precio { get; set; }
    }
    ```

    - Si queremos podemos definir nuestros campos
    ```C#
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; } = null!;
    ```


### Conexion y Migracion a SQL SERVER 

- Despues de definir nuestra clase podemos proseguir a pasar nuestro contexto de datos ara esto sobre la carpeta damos click derecho y `agregar` y seleccionamos `Clase...`
<img src="/assets/img/11.png" width="80%">

- definimos nuestro `Contexto` de nuestra `NameClass`

```C#
//add DbContext para erede de DB
public class NameContext : DbContext
{
    //Constructor
    public NameContext(DbContextOptions<NameContext> options) : base(options)
    {
    }

    //Creamos una propiedad Db Set para nuestra tbl
    public DbSet<NameClass> NameClass { get; set; }

    //Sobreescribimos un metodo _ para que el nombre de cada producto no se repita
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<NameClass>().HasIndex(c => c.Nombre).IsUnique();
    }
}
```
- Ahora nos vamos a configurar nuestra cadena de conexion con `SQL SERVER` para esto abrimos nuestro archivo que se encuentra en raiz con el nombre `appsettings.json` despues de `AllowedHost` definiendo nuestros datos de conexion `Catolog`(Nombre de la BD) `ID`(nombre de usario) `Password`(contraseña)
    -Configuracion SQL SERVER
    ```json
    //Configuramos nuestra cadena de conexion
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=(local);Initial Catalog=NAMEBD;Persist Security Info=True;User ID=USERSQL;Password=PASS-SQL;MultipleActiveResultSets=True;TrustServerCertificate=True"
    ```

    - Configuracion SQL SERVER
    ```json
        //Configuramos nuestra cadena de conexion
        "ConnectionStrings": {
            "DefaultConnection": "Server=DESKTOP-GF7RUMJ\\SQLEXPRESS;Database=NAMEBD;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=false "
    ```

- Despues nos dirigimos a `Program.cs` donde insertamos nuestro contexto de nuesta BD debajado de `builder.Services.AddControllers();`
```C#
    builder.Services.AddControllers();
```
- insertamos nuestro NameContext
```C#
    builder.Services.AddDbContext<NameContext>(o =>
    {
        o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
```
- Contodo esto listo podemos proceder a correr la migracion de base de datos para ello abrimos `Consola del Administrador` que se encuentra en la parte inferior izquierda del programa
    <img src="/assets/img/12.png" width="80%">

- y corremos el siguiente `add-migration NameTbl` comando y esperamos a que realice la migracion de nuestra tabla a la base de datos
```PM
    add-migration NameTbl
```

- volvemos a nuestra consola para acer efectivos nuestros cambios y subirlos a nuestro `SQL SERVER`

```PM
    update-database
```

- Comprobamos que nuesta bd, dirigiendonos a `SQL Server` se creo en sql damos en `New Query`
    ```sql
        use DataBaseName
        select * from Nametbl
    ```
    <img src="/assets/img/13.png" width="80%">


### Creacion de Controladores del Proyecto
- Para empesar nuestro proyecto a controlar agregamos un controlador para nuestra API dando click derecho sobre la carpeta `Controller` moviendonos hasta `Agregar` y Selccionamos `Controlador`
    - <img src="/assets/img/14.png" width="80%">

    - se nos desplegara la siguiente ventana donde Seleccionaremos un Controlador de tipo API `Controlador de API: en blanco` le damos en agregar el le colocamos el nombre de nuestra clase `ClassController`
        - <img src="/assets/img/15.png" width="80%">

- Lo primero que realizamos en nuestro controlador es inyectar nuestro `_context` de nuestro base de datos debajo de nuestra `public class MenuController : ControllerBase`
``` C#
    [Route("api/[controller]")]
    [ApiController]
    public class NameClassContext : ControllerBase
    {
        //insertamos aqui
    }
```
```C#
    //inyectamos contexto de BD 
    private readonly NameClassContext _context;
    public ClassController(NameClassContext context)
    {
        _context = context;
    }
```
- Ya con esto tendremos acceso a la base de datos de cualquier metodo del controlador
    
    -Metodo para Crear en la bd
    ```C#
        [HttpPost]
        [Route("nombreRoute")]
        //Metodo para guardar en BD
        public async Task<IActionResult> CrearElemento(Elemento elemento)
        {
            //guardamos en BD en la tblname, pasando el parametro
            await _context.tblName.AddAsync(elemento);

            //guardamos cambios en BD
            await _context.SaveChangesAsync();

            //returnamos un mensaje de exito
            return Ok();

        }
    ```

    -Metodo `Get` para listar los elementos de la BD
    ```C#
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
    ```

    -Metodo `Get` para visualizar  un solo elemento
    ```C#
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
    ```

    -Metodo `Put` para editar un elemento
    ```C#
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
    ```

    -Metodo `Delete` para eliminar un elemento
    ```C#
        [HttpDelete]
        [Route("eliminar")]
        //Metodo para editar un producto
        public async Task<IActionResult> EliminarProducto(int id, Producto producto)
        {
            var productoeliminado = await _context.Productos.FindAsync(id);

            //aplicamos metod del parametro
            _context.Productos.Remove(productoeliminado);

            await _context.SaveChangesAsync();
            return Ok();

        }
    ```
## Consumir la API desde una WEB

Para consumir la Web-API

- Lo primero que tenemos que hacer es agregar la aplicion web a nuestra Solucion, tendremos que dark click derecho desplazarnos hasta `Agregar` y `Nuevo Proyecto`
    <img src="/assets/img/04.png" width="80%">

    - Seleccionamos el tipo de proyecto  `Aplicacion web ASP.NET Core [Modelo-Vista-Controlador C#]` y le damos click en `Siguiente`
        
        <img src="/assets/img/App_web01.png" width="80%">

    - Definimos el nombre del Proyecto de nuestra aplicacion web y procedemos con click en `Siguiente`
        <img src="/assets/img/App_web03.png" width="80%">

    - Seleccionamos la configuracion deseada en este caso el Framework sera `.NET 8.0` sin Authentication de campo y con la configuracion que viene por defaul, para poder crear la soucion le damos click en `Crear`
        <img src="/assets/img/App_web03.png" width="80%">

- Como primer paso agregamos algo a nuestro `Program.cs` en nuestra solucion web y agregamos un servicio de tipo  `builder.Services.AddHttpClient();` despues de `builder.Services.AddControllersWithViews();`
    ```C#
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        //Insertar aqui
        var app = builder.Build();
    ```
    ```C#
        builder.Services.AddHttpClient();
    ```
- Este servicio lo utilizamos para ser peticiones HTTP y de esta forma poder conectarnos a nuestra web api

- Crearemos un modelo sobre nuestra capeta `Models` damos click derecho y `agregar` y seleccionamos `Clase...` para que sea del tipo `NameClassViewModel`
    <img src="/assets/img/11.png" width="80%">
    - Nos vamos al proyecto nuestra API y copiamos nuestra clase con sus los atrubutos definidos para tener disponible nuestra clase en el proyecto web
    ```C#
    public class NameClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal precio { get; set; }
    }
    ```
- Sobre nuestra carperta `Controller` agregamos un controlador dando click derecho sobre la carpeta `Controller` moviendonos hasta `Agregar` y Selccionamos `Controlador`.
    <img src="/assets/img/14.png" width="80%">

    - se nos desplegara la siguiente ventana donde Seleccionaremos un Controlador de tipo MVC `Controlador de MVC: en blanco` le damos en agregar el le colocamos el nombre de nuestra clase `ClassController`.
        <img src="/assets/img/App_web04.png" width="80%">
    
    -Inyectamos nuestro servicio HTTP en nuestro controlador
    ```C#
        //inyectamos nuestro servicio HTTPCLIENT
        private readonly HttpClient _httpClient;
        public ProductoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            //local host de Productos.Server
            _httpClient.BaseAddress = new Uri("https://localhost:7053/api");//Direccion local de nuestra API
        }
    ```

- Ahora debemos crear nuestros metodos
    - Para nuestro Primer metodo seria para listar nuestra API
    ```C#
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

                return View("Inicio", productos);
            }

            //si no devuelve code 200
            //devolvemos una lsita vavcio
            return View(new List<ProductoViewModel>());

        }
    ```
    - Despues de Tener nuestro Metodo en nuestro Controlador agregamos nuestra vista en este caso seria `Index` para esto debemos dar click derecho sobre ella y seleccionamos `Agregar Vista...`.
        <img src="/assets/img/App_web05.png" width="80%">

        - Seleccionamos una vista MVC de tipo `Vista de Razor`.
            <img src="/assets/img/App_web05.1.png" width="80%">
        
        -Dejamos la configuracion como esta y solo le damos en `agregar`
            <img src="/assets/img/App_web06.png" width="80%">

        -Pasamos el modelo a nuestra vista `index.cshtml` este tiene que esta al inicio del documento
        ```cshtml
            @model IEnumerable<ProductoViewModel>
        ```
        -Para poder obtener el nombre de las columnas de nuestra tabla en nuestro `index.cshtml`las mandamos llamar de esta manera
        ```cshtml
            @Html.DisplayNameFor(model => model.Nombre)
        ```
        -Para listar todos los datos  de nuestra tabla en el `index.cshtml`las mandamos llamar mediante un ciclo.
        ```cshtml
            @foreach (var item in Model)
            {
                <tr>
                    <td>
                        @Html.DisplayFor(modelItem => item.Nombre)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.Descripcion)
                    </td>
                    <td>
                        @Html.DisplayFor(modelItem => item.Precio)
                    </td>

                    <td>
                        <a asp-action="Edit" asp-route-id="@item.Id" class="btn btn-outline-warning btn-sm">Editar</a>
                        <a asp-action="Details" asp-route-id="@item.Id" class="btn btn-outline-info btn-sm">Detalles</a>
                        <a asp-action="Delete" asp-route-id="@item.Id" class="btn btn-outline-danger btn-sm">Eliminar</a>
                    </td>
                </tr>
            }
        ```
    - Para poder correr nuestro programa de nuestro `index.cshtml` de nuestra clase modificamos nuestro archivo `Program.cs` modificando el `app.MapControllerRoute` en nuestro `controller=Home`.
    ```C#
        app.MapControllerRoute(
        name: "default",
        //pattern: "{controller=Home}/{action=Index}/{id?}");
        pattern: "{controller=Producto}/{action=Index}/{id?}");

        app.Run();

    ```
    - Para nuestro metodo de `create` en nuestra API.
        ```C#
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
        ```
        - Y para moder crearlo desde nuestra web creamos una `vista` de razor en nuestro metodo `create` sin configuracion alguna. Ya que tengamos nuestra vista agregamos el modelo `@model Productos.Client.Models.ProductoViewModel`
            ```cshtml
                @model Productos.Client.Models.ProductoViewModel
            ```
        - Para poder guardar los datos de nuestra vista `create.cshtml` creamos un formulario `form asp-action="Create"` y los inputs donde recogemos la informacion `input asp-for="Nombre"` con su respectiva validacion `asp-validation-for="Nombre"`
            ```cshtml
                <form asp-action="Create">
                    <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                    <div class="form-group">
                        <label></label>
                        <input asp-for="Nombre" class="form-control" placeholder="Nombre">
                        <span asp-validation-for="Nombre" class="text-danger"></span>
                    </div>
                    <div class="form-group">
                        <label></label>
                        <input asp-for="Descripcion" class="form-control" placeholder="Descripcion">
                        <span asp-validation-for="Descripcion" class="text-danger"></span>
                    </div>
                    <div class="form-group">
                        <label></label>
                        <input asp-for="Precio" class="form-control" placeholder="Precio">
                        <span asp-validation-for="Precio" class="text-danger"></span>
                    </div>
                    <div class=" card-footer text-center">
                        <div class="form-group">
                            <input type="submit" value="Guardar" class="btn btn-outline-primary btn-sm" />
                            <a asp-action="Index" class="btn btn-outline-success btn-sm">Volver a la Lista</a>
                        </div>
                    </div>
                </form>
            ```
        - Para nuestro metodo de `edith` en nuestra API primero creamos nuestra vista y luego nuestro metodo de tipo `POST` en el cual se le pasan dos parametros `int id` y un objeto del tipo producto `Producto producto`.
        ```C#
            public async Task<IActionResult>Edit(int id)
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
            public async Task<IActionResult>Edit(int id, Producto producto)
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(producto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

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
        ```
        - Y para moder editarlo desde nuestra web creamos una `vista` de razor en nuestro metodo `edit` sin configuracion alguna. Ya que tengamos nuestra inyectamos el modelo `@model Menu.Client.Models.ErrorViewModel` en nuestra vista `edit.cshtml`
        ```cshtml
            @model Menu.Client.Models.ProductoViewModel

            <div class=" card text-primary">
                <div class="card-header text-center">
                    <h5>Editar Producto</h5>
                </div>
                <div class=" card-body text-primary">
                    <div class="row">
                        <div class="col-md-12">
                            <form asp-action="Edit">
                                <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                                <input type="hidden" asp-for="Id" />
                                <div class="form-group">
                                    <label asp-for="Nombre" class="control-label"></label>
                                    <input asp-for="Nombre" class="form-control" />
                                    <span asp-validation-for="Nombre" class="text-danger"></span>
                                </div>
                                <div class="form-group">
                                    <label asp-for="Descripcion" class="control-label"></label>
                                    <textarea asp-for="Descripcion" class="form-control"></textarea>
                                    <span asp-validation-for="Descripcion" class="text-danger"></span>
                                </div>
                                <div class="form-group">
                                    <label asp-for="Precio" class="control-label"></label>
                                    <input asp-for="Precio" class="form-control" />
                                    <span asp-validation-for="Precio" class="text-danger"></span>
                                </div>
                                <div class=" card-footer text-center">
                                    <div class="form-group">
                                        <input type="submit" value="Guardar" class="btn btn-outline-primary btn-sm" />
                                        <a asp-action="Index" class="btn btn-outline-success btn-sm">Volver a la lista</a>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>

            </div>
        ```
    - Para nuestro metodo de `details` en nuestra API, le pasamos un parametro que es nuestro `int id`
    ```C#
        // Monstrar un producto en expecifico
        public async Task<IActionResult> Details(int id)
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
    ```
    - Para poder visualizar nuestro metodo `details` en nuestra web creamos una `vista` de razor en nuestro metodo `details` sin configuracion alguna. Ya que tengamos nuestra inyectamos el modelo `@model Menu.Client.Models.ErrorViewModel` en nuestra vista `edit.cshtml`.
    ```cshtml
        @model Menu.Client.Models.ProductoViewModel
        <div class=" card text-primary">
            <div class=" card-header text-center ">
                <h5>Producto</h5>
            </div>
            <div class=" card-body ">
                <dl class="row">
                    <dt class="col-sm-2 text-center text-dark">
                        @Html.DisplayNameFor(model => model.Nombre)
                    </dt>
                    <dd class="col-sm-10">
                        @Html.DisplayFor(model => model.Nombre)
                    </dd>

                    <dt class="col-sm-2 text-center text-dark">
                        @Html.DisplayNameFor(model => model.Descripcion)
                    </dt>
                    <dd class="col-sm-10">
                        @Html.DisplayFor(model => model.Descripcion)
                    </dd>

                    <dt class="col-sm-2 text-center text-dark">
                        @Html.DisplayNameFor(model => model.Precio)
                    </dt>
                    <dd class="col-sm-10">
                        @Html.DisplayFor(model => model.Precio)
                    </dd>

                </dl>
            </div>
            <div class=" card-footer text-center">
                <a asp-action="Edit" asp-route-id="@Model.Id" class="btn btn-outline-warning btn-sm">Editar</a>
                <a asp-action="Index" class=" btn btn-outline-success btn-sm">volver a la lista</a>
            </div>
        </div>
    ```


    - Para nuestro metodo de `delet` en nuestra API, volvemos a crear un metodo del tipo asincrono donde le pasamos un parametro que es nuestro `int id`
    
    ```C#
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Productos/eliminar?id={id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Erro"] = "Error al eliminar Producto";
                return RedirectToAction("Index");
            }

        }
    ```






<br>
<br>
<br>

**NOTA** para depurara los dos proyecto nos dirigimos a `Iniciar` y luego vamos a `Configurar proyetos de inicio...`.
    
<img src="/assets/img/dep-01.png" width="80%">

- cambiamos de `Selecion actual` a `Varios proyectos de inicio` y seleccionamos los proyetos a ejecutar por el tipo de `Accion` y `Destino de depuracion` y le damos aceptar.
    
    <img src="/assets/img/dep-02.png" width="80%">
