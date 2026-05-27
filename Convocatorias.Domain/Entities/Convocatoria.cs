using Convocatorias.Domain.Enums;


namespace Convocatorias.Domain.Entities
{
    public sealed class Convocatoria
    {
        public Guid Id { get; private set; }
        public int SedeId { get; private set; }
        public int FacultadId { get; private set; }
        public int CarreraId { get; private set; }
        public string Asignatura { get; private set; }
        public Modalidad Modalidad { get; private set; }
        public Status Status { get; private set; }

        private readonly List<ConvocatoriaPeriodo> _periodos = new();
        public IReadOnlyCollection<ConvocatoriaPeriodo> Periodos => _periodos.AsReadOnly();

        private Convocatoria() { }

        public Convocatoria(int sedeId, int facultadId, int carreraId, string asignatura, Modalidad modalidad)
        {
            if (string.IsNullOrWhiteSpace(asignatura))
            {
                throw new ArgumentException("La asignatura no puede estar vacía");
            }

            if (sedeId <= 0 || facultadId <= 0 || carreraId <= 0)
            {
                throw new ArgumentException("Sede, Facultad y Carrera deben ser válidos");
            }
            

           
            SedeId = sedeId;
            FacultadId = facultadId;
            CarreraId = carreraId;
            Asignatura = asignatura;
            Modalidad = modalidad;
            Status = Status.Abierta;

            
        }

        public void CerrarConvocatoria()
        {
            if (Status == Status.Cerrada)
            {
                throw new InvalidOperationException("La convocatoria ya está cerrada");
            }
            Status = Status.Cerrada;
        }

        

        public Guid ObtenerPeriodoActual()
        {
            
            return _periodos.FirstOrDefault(p => p.EsActual)?.PeriodoId ?? Guid.Empty;

        }

        public bool estaAbierta()
        {
            return Status == Status.Abierta;
        }

        public void AgregarPeriodo(Guid periodoId)
        {
            if (periodoId == Guid.Empty)
                throw new ArgumentException("El período debe ser válido.");

            if (_periodos.Any(p => p.PeriodoId == periodoId))
                throw new InvalidOperationException("La convocatoria ya tiene asignado ese período.");

            // Desactiva los actuales en memoria
            foreach (var p in _periodos.Where(p => p.EsActual))
                p.MarcarComoNoActual();

            _periodos.Add(ConvocatoriaPeriodo.Crear(Id, periodoId));
        }

        
    }
}
