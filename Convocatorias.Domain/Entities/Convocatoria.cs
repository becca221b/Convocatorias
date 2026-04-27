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
        public string Modalidad { get; private set; }
        public Status Status { get; private set; } 

        public DateTime FechaInicio { get; private set; }
        public DateTime FechaFin { get; private set; }

        public Convocatoria(int sedeId, int facultadId, int carreraId, string asignatura, string modalidad, DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaFin <= fechaInicio) {
                throw new ArgumentException("Fecha de fin debe ser mayor a la fecha de inicio");
            }

            if (string.IsNullOrWhiteSpace(asignatura)) {
                throw new ArgumentException("La asignatura no puede estar vacía");
            }

            if (string.IsNullOrWhiteSpace(modalidad)) {
                throw new ArgumentException("La modalidad no puede estar vacía");
            }

            if(sedeId <= 0 || facultadId <= 0 || carreraId <= 0) {
                throw new ArgumentException("Sede, Facultad y Carrera deben ser válidos");
            }

            Id = Guid.NewGuid();
            SedeId = sedeId;
            FacultadId = facultadId;
            CarreraId = carreraId;
            Asignatura = asignatura;
            Modalidad = modalidad;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Status= Status.Activa;
        }

        public void CerrarConvocatoria()
        {
            Status = Status.Cerrada;
        }

        public bool EstaDisponible()
        {
            var now = DateTime.UtcNow;
            return now <= FechaFin && Status == Status.Activa;
        }

        public void ReabrirConvocatoria( DateTime nuevaFechaInicio, DateTime nuevaFechaFin)
        {
            if (nuevaFechaFin <= nuevaFechaInicio) {
                throw new ArgumentException("La nueva fecha de fin no puede ser menor al inicio");
            }

            if (nuevaFechaFin < DateTime.UtcNow) {
                throw new ArgumentException("La nueva fecha fin ya pasó");
            }
            
            if(Status == Status.Cerrada) {
                throw new InvalidOperationException("No se puede modificar el período de una convocatoria cerrada");
            }

            FechaInicio = nuevaFechaInicio;
            FechaFin = nuevaFechaFin;
        }
    }
}
