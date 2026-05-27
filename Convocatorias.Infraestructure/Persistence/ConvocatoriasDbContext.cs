using Convocatorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;


namespace Convocatorias.Infraestructure.Persistence
{
    public sealed class ConvocatoriasDbContext : DbContext
    {
        public ConvocatoriasDbContext(DbContextOptions<ConvocatoriasDbContext> options) : base(options) { }
        
        public DbSet<Convocatoria> Convocatorias => Set<Convocatoria>();
        public DbSet<Periodo> Periodos => Set<Periodo>();

        

        public DbSet<Postulacion> Postulaciones => Set<Postulacion>();
        
        public DbSet<Candidato> Candidatos => Set<Candidato>();
        
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ConvocatoriasDbContext).Assembly);

        }
    }  

}
