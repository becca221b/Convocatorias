using Convocatorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Convocatorias.Infraestructure.Persistence.Configurations
{
    internal sealed class ConvocatoriaPeriodoConfiguracion
        : IEntityTypeConfiguration<ConvocatoriaPeriodo>
    {
        public void Configure(EntityTypeBuilder<ConvocatoriaPeriodo> builder)
        {
            builder.ToTable("ConvocatoriaPeriodos");

            builder.HasKey(cp => new
            {
                cp.ConvocatoriaId,
                cp.PeriodoId
            });

            builder.Property(cp => cp.EsActual)
                .IsRequired();
            builder.Property(cp => cp.AsignadoEn)
                .IsRequired();

            builder.HasOne(cp => cp.Convocatoria)
                .WithMany(c => c.Periodos)
                .HasForeignKey(cp => cp.ConvocatoriaId);



            builder.HasOne(cp => cp.Periodo)
                .WithMany(p => p.Convocatorias)
                .HasForeignKey(cp => cp.PeriodoId);


            builder.HasIndex(x => new
            {
                x.ConvocatoriaId,
                x.PeriodoId
            })
            .IsUnique();
        }

        
    }
}