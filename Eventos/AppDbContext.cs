using Eventos.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
        public DbSet<Eventos.Models.Pregunta> Preguntas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pregunta>()
                .HasKey(p => p.Id); // Define explícitamente la clave primaria
        }
    }
}
