using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Domain.Entities
{
    internal class Convocatoria
    {
        public int Id { get; set; }
        public string Sede { get; set; }
        public string Facultad { get; set; }
        public string Carrera { get; set; }
        public string Asignatura { get; set; }
        public string Modalidad { get; set; }
    }
}
