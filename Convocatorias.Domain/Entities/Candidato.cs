namespace Convocatorias.Domain.Entities
{
    public sealed class Candidato
    {
       public Guid Id { get; private set; }
       public string Nombre { get; private set; }
       public string Apellido { get; private set; }
       public string Email { get; private set; }

       private readonly List<Educacion> _educaciones = new ();
       public IReadOnlyCollection<Educacion> Educaciones => _educaciones;

       // private readonly List<Experiencia> _experiencias = new ();
       // public IReadOnlyCollection<Experiencia> Experiencias => _experiencias;

        public Candidato(string nombre, string apellido, string email)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
            
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido no puede estar vacío.", nameof(apellido));
            
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("El email no es válido.", nameof(email));

            Id = Guid.NewGuid();
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
        }

        public void AgregarEducacion(Educacion educacion)
        {
            if (educacion == null)
                throw new ArgumentNullException(nameof(educacion), "La educación no puede ser nula.");
            _educaciones.Add(educacion);
        }

        /*
        public void AgregarExperiencia(Experiencia experiencia)
        {
            // Implementar lógica para agregar experiencia laboral
        }*/
    }
}
