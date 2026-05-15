using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Domain.Entities
{
    public sealed class Facultad
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; }
    }
}
