using Convocatorias.Domain.Enums;


namespace Convocatorias.Domain.Entities
{
    public class Documento
    {
        public Guid Id { get; private set; }
        public string Tipo { get; private set; } // Ejemplo: "cv", "Certificado", "Constancia".
        public string Url { get; private set; } // Ruta donde se almacena el archivo
        
        public TipoArea TipoArea { get; private set; } // Ejemplo: "Experiencia Docente", "Educacion", "Experiencia No Universitaria", "Investigacion", "Extensión"
    
        public int AreaId { get; private set; }

        public Documento(TipoArea tipoArea, int areaId, string tipo, string url)
        {
            Id = Guid.NewGuid();
            TipoArea = tipoArea;
            AreaId = areaId;
            Tipo = tipo;
            Url = url;
        }

    }
}
