using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Postularse
{
    public class PostularseRequest
    {
        public Guid ConvocatoriaId { get; set; }
        public Guid CandidatoId { get; set; }
    }
}
