
using Convocatorias.Domain.Enums;

namespace Convocatorias.Domain.Entities
{
    public sealed class Educacion
    {
        public Guid Id { get; private set; }
        public string TituloGrado { get; private set; }
        public int AnioGraduacion { get; private set; }

        public PosgradoStatus PosgradoStatus { get; private set; } // Ejemplo: "En curso", "Completado", "No aplica"

        public string PosgradoNombre { get; private set; } // Nombre del posgrado, si aplica
        public string TipoFormacion { get; private set; }

        private readonly List<Documento> _documentos = new List<Documento>();
        public IReadOnlyCollection<Documento> Documentos => _documentos.AsReadOnly();

        private Educacion() { }

        public Educacion(string titulo, int anioGraduacion, PosgradoStatus posgradoStatus, string posgradoNombre, string tipoFormacion)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El título del grado no puede estar vacío.", nameof(titulo));
            if (anioGraduacion < 1900 || anioGraduacion > DateTime.UtcNow.Year)
                throw new ArgumentException("El año de graduación no es válido.", nameof(anioGraduacion));
            

            Id = Guid.NewGuid();
            TituloGrado = titulo;
            AnioGraduacion = anioGraduacion;
            PosgradoStatus = posgradoStatus;
            PosgradoNombre = posgradoNombre;
            TipoFormacion = tipoFormacion;
        }

        public void AgregarDocumento(Documento documento)
        {
            if (documento == null)
                throw new ArgumentNullException(nameof(documento));

            _documentos.Add(documento);
        }
    }
}
