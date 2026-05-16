using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.AprobarPostulaciones
{
    public class AprobarPostulacionRequest
    {
        public Guid ConvocatoriaId { get; set; }
        public Guid PostulacionId { get; set; }
    }
}
