# Crear WEB´S API con .NET y SQL SERVER

## Creacion de una Web api con asp.net

## Creacion de la API
### Inicializacion de un Proyecto

- Para inicializar un proyecto en Visual Studio lo primero que tenemos que realizar es dirigimos a `Crear un Proyecto`
    - <img src="/assets/img/01.png" width="80%">

- Posteriormente seleccionamos  el tipo de proyecto que necesitemos en este caso seleccionamos una `Solucion en Blanco` y le damos en `siguiente`
    - <img src="/assets/img/02.png" width="80%">

- Llegando hasta este paso definimos el nombre del proyecto y le damos en `Crear`
<img src="/assets/img/03.png" width="80%">

- Para empezar a crear nuestra API damos click derecho sobre nuestra soluccion nos deslizamos hasta la pestaña `Agregar` y seleccionamos  `Nuevo proyecto`
<img src="/assets/img/04.png" width="80%">

- Seleccionamos el tipo de proyecto  `ASP.NET Core Web API C#` y le damos click en `Siguiente`
<img src="/assets/img/05.png" width="80%">

- Definimos el nombre del Proyecto de nuestra  que sera el servidor de nuestra  Web API le damos click en `Siguiente`
<img src="/assets/img/06.png" width="80%">

- Seleccionamos la configuracion deseada en este caso el Framework sera `.NET 8.0` sin Authentication de campo y con la configuracion que viene por defaul, para poder crear la soucion le damos click en `Crear`
<img src="/assets/img/07.png" width="80%">

- Para poder trabajar con `SQL SERVER` necesitamos instalar algunos packetes para esto damos click derecho en nuestro proyecto, y nos deslizamos hasta donde este `Administrar paquetes NuGet para la solucion...`
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
```json
//Configuramos nuestra cadena de conexion
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(local);Initial Catalog=NAMEBD;Persist Security Info=True;User ID=USERSQL;Password=PASS-SQL;MultipleActiveResultSets=True;TrustServerCertificate=True"

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
## Consumir la APi desde una WEB
