using Convocatorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Convocatorias.Infraestructure.Persistence.Configurations
{
    internal sealed class ConvocatoriaConfiguracion 
        : IEntityTypeConfiguration<Convocatoria>
    {
        public void Configure(EntityTypeBuilder<Convocatoria> builder)
        {
            builder.ToTable("Convocatorias");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.SedeId)
                .IsRequired();
            builder.Property(c => c.FacultadId)
                .IsRequired();
            builder.Property(c => c.CarreraId)
                .IsRequired();
            builder.Property(c => c.Asignatura)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(c => c.Modalidad)
                .IsRequired();
            builder.Property(c => c.Status)
                .IsRequired();
            builder.HasMany(c => c.Periodos)
                .WithOne(cp => cp.Convocatoria)
                .HasForeignKey(cp => cp.ConvocatoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        
    }
}