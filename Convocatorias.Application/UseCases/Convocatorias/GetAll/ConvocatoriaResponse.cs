using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Convocatorias.GetAll
{
    public sealed record ConvocatoriaResponse
    (
        Guid Id,
        string Asignatura,
        int SedeId,
        int FacultadId,
        int CarreraId,
        string Modalidad,
        string Status
    );
}
