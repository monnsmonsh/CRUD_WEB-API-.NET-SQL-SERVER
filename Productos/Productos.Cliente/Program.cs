using Microsoft.AspNetCore.Mvc;

namespace Productos.Cliente
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            //
            ///add servicio de AddHttpClient ->para poder conectar a web api
            builder.Services.AddHttpClient();
            //


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                 //modificamos la ruta para que nos aranque desde nuestro Controlador index
                 //pattern: "{controller=Home}/{action=Index}/{id?}");
                 pattern: "{controller=Producto}/{action=create}/{id?}");

            app.Run();
        }

        
    }
}
