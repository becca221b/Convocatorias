

using Convocatorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convocatorias.Infraestructure.Persistence.Configurations
{
    internal sealed class ExperienciaDocenteConfiguracion : IEntityTypeConfiguration<ExperienciaDocente>
    {
        public void Configure(EntityTypeBuilder<ExperienciaDocente> builder)
        {
            builder.ToTable("ExperienciasDocentes");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Cargo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Institucion)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.AniosExperiencia)
                .IsRequired();

            builder.Property(e => e.Nivel)
                .HasConversion<string>().HasMaxLength(20);

            builder.Property(e=> e.Institucion)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.DesdePeriodo)
                .IsRequired();

            builder.Property(e => e.HastaPeriodo)
                .IsRequired();

            builder.HasMany(e => e.Documentos)
                .WithOne()
                .HasForeignKey("ExperienciaDocenteId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(e => e.Documentos)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }

       
    }
}