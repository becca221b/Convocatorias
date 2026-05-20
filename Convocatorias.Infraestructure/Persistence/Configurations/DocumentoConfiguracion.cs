

using Convocatorias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Convocatorias.Infraestructure.Persistence.Configurations
{
    internal sealed class DocumentoConfiguracion : IEntityTypeConfiguration<Documento>
    {
        public void Configure(EntityTypeBuilder<Documento> builder)
        {
            builder.ToTable("Documentos");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.TipoDocumento)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(d => d.Url)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
