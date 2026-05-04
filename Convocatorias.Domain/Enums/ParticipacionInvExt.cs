using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Domain.Enums
{
    public enum ParticipacionInvExt
    {
        Director = 1,
        Asistente = 2,
        Colaborador = 3,
        Ninguno = 4 // Para casos donde no tiene experiencia, se asigna "Ninguno"
    }
}
