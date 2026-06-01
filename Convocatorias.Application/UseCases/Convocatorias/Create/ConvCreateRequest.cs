using Convocatorias.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Convocatorias.Create
{
    public sealed record ConvCreateRequest(
        int SedeId,
        int FacultadId,
        int CarreraId,
        string Asignatura,
        Modalidad Modalidad
    );
}
