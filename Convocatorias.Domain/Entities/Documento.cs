using Convocatorias.Domain.Enums;


namespace Convocatorias.Domain.Entities
{
    public sealed class Documento
    {
        public Guid Id { get; private set; }
        public string TipoDocumento { get; private set; } // Ejemplo: "cv", "Certificado", "Constancia".
        public string Url { get; private set; } // Ruta donde se almacena el archivo
        
        

        private Documento() { }

        public Documento(  string tipo, string url)
        {
                if (string.IsNullOrWhiteSpace(tipo))
                    throw new ArgumentException("El tipo de documento no puede estar vacío.", nameof(tipo));
                
                if (string.IsNullOrWhiteSpace(url))
                    throw new ArgumentException("La URL del documento no puede estar vacía.", nameof(url));
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    throw new ArgumentException("La URL del documento no es válida.", nameof(url));
            Id = Guid.NewGuid();
            TipoDocumento = tipo;
            Url = url;
        }

    }
}
