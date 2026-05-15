using Convocatorias.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Domain.Entities
{
    public sealed class ExperienciaInvExt
    {
        public Guid Id { get; private set; }

        public TipoExperiencia Tipo { get; private set; } // Extension | Investigacion

        public bool TieneExperiencia { get; private set; }
        public ParticipacionInvExt ParticipoComo { get; private set; } // Director | Asistente | Colaborador
        public string Descripcion { get; private set; }

        private readonly List<Documento> _documentos = new();
        public IReadOnlyCollection<Documento> Documentos => _documentos;

        private ExperienciaInvExt() { }

        public ExperienciaInvExt(TipoExperiencia tipo, bool tieneExperiencia, ParticipacionInvExt participoComo, string descripcion)
        {
            Id = Guid.NewGuid();
            Tipo = tipo;
            TieneExperiencia = tieneExperiencia;
            if(tieneExperiencia == false)
            {
                participoComo = ParticipacionInvExt.Ninguno;
                descripcion = string.Empty;
            }
            else
            {
                ParticipoComo = participoComo;
                Descripcion = descripcion;
            }
            

        }
    }
}
