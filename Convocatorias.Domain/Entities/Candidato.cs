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

       private readonly List<ExperienciaDocente> _experienciasDocente = new ();
       public IReadOnlyCollection<ExperienciaDocente> ExperienciasDocente => _experienciasDocente;

        private readonly List<ExperienciaInvExt> _experienciasInvExt = new ();
        public IReadOnlyCollection<ExperienciaInvExt> ExperienciasInvExt => _experienciasInvExt;

        private Candidato() { }
        public Candidato(string nombre, string apellido, string email)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
            
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido no puede estar vacío.", nameof(apellido));

            if (!System.Net.Mail.MailAddress.TryCreate(email, out _))
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

        public void AgregarExperienciaDocente(ExperienciaDocente experiencia)
        {
            if (experiencia == null)
                throw new ArgumentNullException(nameof(experiencia), "La experiencia docente no puede ser nula.");
            _experienciasDocente.Add(experiencia);
        }

        public void AgregarExperienciaInvExt(ExperienciaInvExt experiencia)
        {
            _experienciasInvExt.Add(experiencia);
        }
    }
}
