using Convocatorias.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Educacion.Get
{
    public sealed record EducacionResponse
    (
        
        string Titulo,
        int AnioGraduacion,
        PosgradoStatus PosgradoStatus,
        string PosgradoNombre,
        string TipoFormacion

    );
}
