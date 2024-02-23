using Microsoft.EntityFrameworkCore;

namespace Productos.Server.Models
{
    //add DbContext
    public class ProductosContext :DbContext
    {
        //Constructor
        public ProductosContext(DbContextOptions<ProductosContext> options) : base(options)
        {
        }

        //Creamos una propiedad Db Set
        public DbSet<Producto> Productos { get; set; }

        //Sobreescribimos un metodo _ para que el nombre de cada producto no se repita
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Producto>().HasIndex(c => c.Nombre).IsUnique();
        }
    }
}
