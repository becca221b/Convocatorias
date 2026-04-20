using Convocatorias.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Domain.Entities
{
    internal class Documento
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // Ejemplo: "cv", "Certificado", "Constancia".
        public string Url { get; set; } // Ruta donde se almacena el archivo
        
        public TipoArea TipoArea { get; set; } // Ejemplo: "Experiencia Docente", "Educacion", "Experiencia No Universitaria", "Investigacion", "Extensión"
    
        public int AreaId { get; set; }

        // Relacion con Candidato
    }
}
