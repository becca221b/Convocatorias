using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Domain.Entities
{
    internal class Carrera
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int FacultadId { get; set; }
       

        //Relaciones con Facultad
       
        public Facultad Facultad { get; set; }

    }
}
