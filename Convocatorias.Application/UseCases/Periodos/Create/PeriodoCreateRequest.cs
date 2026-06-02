using Convocatorias.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Periodos.Create
{
    public sealed record PeriodoCreateRequest
    (
        int Orden,
        Cuatrimestre Cuatrimestre,
        int Anio,
        DateTime FechaInicio,
        DateTime FechaFin
    );
}
