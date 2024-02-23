using Microsoft.EntityFrameworkCore;

namespace Menu.Server.Models
{
    //add DbContext para erede de DB
    public class MenuContext : DbContext
    {
        //Constructor
        public MenuContext(DbContextOptions<MenuContext> options) : base(options)
        {
        }

        //Creamos una propiedad Db Set para nuestra tbl
        public DbSet<Menu> Menu { get; set; }

        //Sobreescribimos un metodo _ para que el nombre de cada producto no se repita
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Menu>().HasIndex(c => c.Nombre).IsUnique();
        }
    }
}