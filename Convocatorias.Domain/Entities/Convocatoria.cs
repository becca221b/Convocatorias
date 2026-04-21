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
        public int SedeId { get; set; }
        public int FacultadId { get; set; }
        public int CarreraId { get; set; }
        public string Asignatura { get; set; }
        public string Modalidad { get; set; }

        //Relaciones con Sede, Facultad y Carrera
        public Sede Sede { get; set; }
        public Facultad Facultad { get; set; }
        public Carrera Carrera { get; set; }

        //Coleccion de postulaciones
        public ICollection<Postulacion> Postulaciones { get; set; } = new List<Postulacion>();

    }
}
