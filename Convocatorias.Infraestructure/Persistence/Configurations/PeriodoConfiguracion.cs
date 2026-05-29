using Convocatorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Convocatorias.Infraestructure.Persistence.Configurations
{
    internal sealed class PeriodoConfiguracion : IEntityTypeConfiguration<Periodo>
    {
       
        public void Configure(EntityTypeBuilder<Periodo> builder)
        {
            builder.ToTable("Periodos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Orden)
                .IsRequired();
            builder.Property(p => p.Cuatrimestre);
            builder.Property(p => p.Anio)
                .IsRequired();
            builder.Property(p => p.FechaInicio)
                .IsRequired();
            builder.Property(p => p.FechaFin)
                .IsRequired();
            
        }
    }
}
