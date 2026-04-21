using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Domain.Entities
{
    internal class Postulacion
    {
        public int Id { get; set; }
        public int ConvocatoriaId { get; set; }
        public int CandidatoId { get; set; }
        public DateTime FechaPostulacion { get; set; }

        // Relaciones
        public Convocatoria Convocatoria { get; set; }
        public Candidato Candidato { get; set; }
    }
}
