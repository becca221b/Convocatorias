using Convocatorias.Application.Interfaces;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Infraestructure.Persistence;
using Convocatorias.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Infraestructure.DependencyInjection
{
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddRepositories();
            services.AddStorageServices(configuration);
            return services;
        }


        // ─── Base de datos ────────────────────────────────────────────────────────
        // Program.cs no sabe que es PostgreSQL. Si mañana cambiás a SQL Server u otro,
        // solo se toca este método — ninguna otra capa se entera.

        private static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException(
                    "Connection string 'Postgres' no encontrada en la configuración.");

            //Aquí va la lógica para configurar la base de datos utilizando el connectionString
            services.AddDbContext<ConvocatoriasDbContext>(options =>
                options.UseNpgsql(connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly("Convocatorias.Infraestructure");
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorCodesToAdd: null);
                    }));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        // ─── Repositorios ─────────────────────────────────────────────────────────
        // Mapea cada interfaz (Application) a su implementación concreta (Infrastructure).
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Aquí se agregan los repositorios concretos
            services.AddScoped<IConvocatoriaRepository, ConvocatoriaRepository>();
            services.AddScoped<ICandidatoRepository, CandidatoRepository>();
            services.AddScoped<IPeriodoRepository, PeriodoRepository>();
            services.AddScoped<IPostulacionRepository, PostulacionRepository>();

            return services;
        }

        // ─── Storage (S3) ────────────────────────────────────────────────────────
        // Program.cs no sabe que el storage es S3.
        // Application solo conoce IDocumentoStorageService (definido en Application).
        // Si cambiás a otro Storage, solo se reemplaza S3DocumentoStorageService
        // aquí — Application y Domain no se tocan.

        private static IServiceCollection AddStorageServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Aquí se agrega la implementación concreta del servicio de almacenamiento
            // Descomenta cuando implementes S3DocumentoStorageService:
            //
            // var awsOptions = configuration.GetSection("Aws");
            //
            // services.AddSingleton<IAmazonS3>(_ =>
            //     new AmazonS3Client(
            //         awsOptions["AccessKeyId"],
            //         awsOptions["SecretAccessKey"],
            //         RegionEndpoint.GetBySystemName(awsOptions["Region"])));
            //

            // services.AddScoped<IDocumentoStorageService, S3DocumentoStorageService>();
            return services;
        }
    }
}
