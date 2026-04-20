using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Domain.Entities
{
    internal class Educacion
    {
        public int Id { get; set; }
        public string TituloGrado { get; set; }
        public int AnioGraduacion { get; set; }
        
        public string PosgradoStatus { get; set; } // Ejemplo: "En curso", "Completado", "No aplica"

        public string PosgradoNombre { get; set; } // Nombre del posgrado, si aplica
        public string TipoFormacion { get; set; }

        // Relación con Candidato
        public int CandidatoId { get; set; }
        public Candidato Candidato { get; set; }

        //Colecciones
        public ICollection<Documento> Documentos { get; set; }
    }
}
