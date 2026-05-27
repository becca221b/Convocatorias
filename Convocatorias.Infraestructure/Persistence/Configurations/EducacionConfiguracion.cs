using Convocatorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convocatorias.Infraestructure.Persistence.Configurations
{
    internal sealed class EducacionConfiguracion : IEntityTypeConfiguration<Educacion>
    {
        public void Configure(EntityTypeBuilder<Educacion> builder)
        {
            builder.ToTable("Educaciones");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.TituloGrado)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.AnioGraduacion)
                .IsRequired();

            builder.Property(e => e.PosgradoStatus);

            builder.Property(e => e.PosgradoNombre)
                .HasMaxLength(200);
            builder.Property(e => e.TipoFormacion)
                .HasMaxLength(200);

            builder.HasMany(e => e.Documentos)
                .WithOne()
                .HasForeignKey("EducacionId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(e => e.Documentos)
                .HasField("_documentos")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }

        
    }
}
