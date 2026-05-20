using Convocatorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Convocatorias.Infraestructure.Persistence.Configurations
{
    internal sealed class ExperienciaInvExtConfiguracion : IEntityTypeConfiguration<ExperienciaInvExt>
    {
        public void Configure(EntityTypeBuilder<ExperienciaInvExt> builder)
        {
            builder.ToTable("ExperienciasInvExt");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Tipo)
                .HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.ParticipoComo)
                .HasConversion<string>().HasMaxLength(20);
            builder.Property(e => e.Descripcion)
                .HasMaxLength(500);

            builder.HasMany(e => e.Documentos)
                .WithOne()
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(e => e.Documentos)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}