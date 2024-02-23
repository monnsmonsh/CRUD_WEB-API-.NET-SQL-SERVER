# Crear WEB´S API con .NET y SQL SERVER

## Creacion de una Web api con asp.net


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
