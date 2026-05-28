using Convocatorias.Application.UseCases.AprobarPostulaciones;
using Convocatorias.Application.UseCases.AsignarPeriodoAConvocatoria;
using Convocatorias.Application.UseCases.Postularse;
using Microsoft.Extensions.DependencyInjection;


namespace Convocatorias.Application.DependencyInjection
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            // Aquí se agregarían servicios específicos de la capa Application, como:
            // - Servicios de dominio
            // - Validadores
            // - Mappers (AutoMapper, etc.)
            // - Otros servicios relacionados con la lógica de negocio
            services.AddUseCases();
            return services;
        }

        private static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<PostularseUseCase>();
            services.AddScoped<AprobarPostulacionUseCase>();
            services.AddScoped<AsignarPeriodo>();

            // A medida que agreguemos use cases, los registramos acá.
            // Program.cs nunca necesita saber que existen.
            return services;
        }
    }
}
