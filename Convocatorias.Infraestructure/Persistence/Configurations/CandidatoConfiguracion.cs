
using Convocatorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convocatorias.Infraestructure.Persistence.Configurations
{
    internal sealed class CandidatoConfiguracion : IEntityTypeConfiguration<Candidato>
    {
        public void Configure(EntityTypeBuilder<Candidato> builder)
        {
            builder.ToTable("Candidatos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.HasMany(c=> c.Educaciones)
                .WithOne()
                .HasForeignKey("CandidatoId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.ExperienciasDocente)
                .WithOne()
                .HasForeignKey("CandidatoId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.ExperienciasInvExt)
                .WithOne()
                .HasForeignKey("CandidatoId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(c => c.Educaciones)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(c => c.ExperienciasDocente)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(c => c.ExperienciasInvExt)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }

        
    }
}