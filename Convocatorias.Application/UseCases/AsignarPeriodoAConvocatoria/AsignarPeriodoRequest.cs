using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.AsignarPeriodoAConvocatoria
{
    public class AsignarPeriodoRequest
    {
        public Guid ConvocatoriaId { get; set; }
        public Guid PeriodoId { get; set; }
    }
}
