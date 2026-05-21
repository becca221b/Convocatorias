using Convocatorias.Domain.Enums;


namespace Convocatorias.Domain.Entities
{
    public sealed class ExperienciaDocente
    {
        public Guid Id { get; private set; }
        public int AniosExperiencia { get; private set; }
        public Nivel Nivel { get; private set; }
        public string Institucion { get; private set; }
        public string Cargo { get; private set; }
        public DateTime DesdePeriodo { get; private set; }
        public DateTime HastaPeriodo { get; private set; }


        private readonly List<Documento> _documentos = new List<Documento>();
        public IReadOnlyCollection<Documento> Documentos => _documentos.AsReadOnly();

        private ExperienciaDocente() { }

        public ExperienciaDocente(int aniosExperiencia, Nivel nivel, string institucion, string cargo, DateTime desdePeriodo, DateTime hastaPeriodo)
        {
            if (aniosExperiencia < 0)
                throw new ArgumentException("Los años de experiencia no pueden ser negativos.", nameof(aniosExperiencia));
            if (string.IsNullOrWhiteSpace(institucion))
                throw new ArgumentException("La institución no puede estar vacía.", nameof(institucion));
            if (string.IsNullOrWhiteSpace(cargo))
                throw new ArgumentException("El cargo no puede estar vacío.", nameof(cargo));
            if (desdePeriodo > hastaPeriodo)
                throw new ArgumentException("La fecha de inicio del periodo no puede ser posterior a la fecha de fin del periodo.", nameof(desdePeriodo));

            Id = Guid.NewGuid();
            AniosExperiencia = aniosExperiencia;
            Nivel = nivel;
            Institucion = institucion;
            Cargo = cargo;
            DesdePeriodo = desdePeriodo;
            HastaPeriodo = hastaPeriodo;
        }

        public void AgregarDocumento(Documento documento)
        {
            if (documento == null)
                throw new ArgumentNullException(nameof(documento));

            _documentos.Add(documento);
        }
    }
}
