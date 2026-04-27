using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Domain.Entities
{
    public sealed class Educacion
    {
        public Guid Id { get; set; }
        public string TituloGrado { get; set; }
        public int AnioGraduacion { get; set; }

        public string PosgradoStatus { get; set; } // Ejemplo: "En curso", "Completado", "No aplica"

        public string PosgradoNombre { get; set; } // Nombre del posgrado, si aplica
        public string TipoFormacion { get; set; }

        public Educacion(string titulo, int anioGraduacion, string posgradoStatus, string posgradoNombre, string tipoFormacion)
        {
            Id = Guid.NewGuid();
            TituloGrado = titulo;
            AnioGraduacion = anioGraduacion;
            PosgradoStatus = posgradoStatus;
            PosgradoNombre = posgradoNombre;
            TipoFormacion = tipoFormacion;
        }
    }
}
