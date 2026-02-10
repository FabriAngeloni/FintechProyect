using AssetService.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetService.Data
{
    public class AssetDbContext : DbContext
    {
        public AssetDbContext(DbContextOptions<AssetDbContext> options) : base(options)
        {
        }

        public DbSet<Activo> Activos { get; set; }
        public DbSet<ActivoDeUser> ActivosDeUsuario { get; set; }


        //Con este metodo creo la columna TipoDeActivo donde voy a poder almacenar si lo que tenemos es una accion, bono o un fondo.
        //.Entity<Activo> apunta a la tabla Activos por convencion.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activo>()
                .HasDiscriminator<string>("TipoDeActivo")
                .HasValue<Accion>("Accion")
                .HasValue<Bono>("Bono")
                .HasValue<Fondo>("Fondo");
        }
    }
}
