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

        public Convocatoria(int sedeId, int facultadId, int carreraId, string asignatura, Modalidad modalidad, Guid periodoInicialId)
        {
            if (string.IsNullOrWhiteSpace(asignatura))
            {
                throw new ArgumentException("La asignatura no puede estar vacía");
            }

            if (sedeId <= 0 || facultadId <= 0 || carreraId <= 0)
            {
                throw new ArgumentException("Sede, Facultad y Carrera deben ser válidos");
            }
            if (periodoInicialId == Guid.Empty)
            {
                throw new ArgumentException("El período inicial debe ser válido");
            }

            Id = Guid.NewGuid();
            SedeId = sedeId;
            FacultadId = facultadId;
            CarreraId = carreraId;
            Asignatura = asignatura;
            Modalidad = modalidad;
            Status = Status.Abierta;

            _periodos.Add(ConvocatoriaPeriodo.Crear(Id, periodoInicialId));
        }

        public void CerrarConvocatoria()
        {
            if (Status == Status.Cerrada)
            {
                throw new InvalidOperationException("La convocatoria ya está cerrada");
            }
            Status = Status.Cerrada;
        }


        public void ModificarPeriodo(Guid nuevoPeriodoId)
        {
            if (Status == Status.Cerrada)
            {
                throw new InvalidOperationException("No se puede modificar el período de una convocatoria cerrada");
            }
            if (nuevoPeriodoId == Guid.Empty)
            {
                throw new ArgumentException("El período debe ser válido");
            }

            foreach (var periodo in _periodos)
            {
                periodo.MarcarComoNoActual();
            }

            _periodos.Add(ConvocatoriaPeriodo.Crear(Id, nuevoPeriodoId));
        }

        public Guid ObtenerPeriodo(Guid periodoId)
        {
            
            return _periodos.FirstOrDefault(p => p.PeriodoId == periodoId)?.PeriodoId ?? Guid.Empty;


        }

        public void ValidarAbierta()
        {
            if (Status != Status.Abierta)
                throw new Exception("La convocatoria está cerrada");
        }
    }
}
