using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.AprobarPostulaciones
{
    public class AprobarPostulacionResponse
    {
        public Guid PostulacionId { get; set; }
        public string CandidatoName { get; set; }
        public string Estado { get; set; }
        public string ConvocatoriaName { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
