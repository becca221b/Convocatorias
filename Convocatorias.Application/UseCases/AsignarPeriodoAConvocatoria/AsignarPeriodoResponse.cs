using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.AsignarPeriodoAConvocatoria
{
    public class AsignarPeriodoResponse
    {
        public Guid ConvocatoriaId { get; set; }
        public Guid PeriodoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public String PeriodoNombre { get; set; }
    }
}
