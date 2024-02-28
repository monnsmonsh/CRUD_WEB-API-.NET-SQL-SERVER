using Servicios.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Servicios.Server.Models
{
    //add DbContext
    public class ServiciosContext : DbContext
    {
        //Constructor
        public ServiciosContext(DbContextOptions<ServiciosContext> options) : base(options)
        {
        }

        //Creamos una propiedad Db Set
        public DbSet<Servicio> Servicios { get; set; }

        //Sobreescribimos un metodo _ para que el nombre de cada producto no se repita
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Servicio>().HasIndex(c => c.Nombre).IsUnique();
        }
    }
}
