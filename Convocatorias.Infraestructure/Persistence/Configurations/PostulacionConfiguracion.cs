using Convocatorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convocatorias.Infraestructure.Persistence.Configurations
{
    internal sealed class PostulacionConfiguracion : IEntityTypeConfiguration<Postulacion>
    {
        public void Configure(EntityTypeBuilder<Postulacion> builder)
        {
            builder.ToTable("Postulaciones");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ConvocatoriaId)
                .IsRequired();

            builder.Property(p => p.CandidatoId)
                .IsRequired();

            builder.Property(p => p.FechaPostulacion)
                .IsRequired();
                
            builder.Property(p => p.Estado)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // Índice único: un candidato no puede postularse dos veces a la misma convocatoria.
            // REFORZAR en BD y validación en DOMINIO.
            builder.HasIndex(p => new { p.ConvocatoriaId, p.CandidatoId })
                .IsUnique();

            builder.HasOne<Convocatoria>()
                .WithMany()
                .HasForeignKey(p => p.ConvocatoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Candidato>()
                .WithMany()
                .HasForeignKey(p => p.CandidatoId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
