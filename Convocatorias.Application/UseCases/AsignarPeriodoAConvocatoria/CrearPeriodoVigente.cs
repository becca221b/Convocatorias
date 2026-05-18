using Convocatorias.Application.Interfaces;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;


namespace Convocatorias.Application.UseCases.AsignarPeriodoAConvocatoria
{
    public class CrearPeriodoVigente
    {
        private readonly IConvPeriodoRepository _convPeriodoRepository;
        private readonly IConvocatoriaRepository _convocatoriaRepository;
        private readonly IPeriodoRepository _periodoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CrearPeriodoVigente(IConvPeriodoRepository convPeriodoRepository, IConvocatoriaRepository convocatoriaRepository, IPeriodoRepository periodoRepository, IUnitOfWork unitOfWork)
        {
            _convPeriodoRepository = convPeriodoRepository;
            _convocatoriaRepository = convocatoriaRepository;
            _periodoRepository = periodoRepository;
            _unitOfWork = unitOfWork;
        }

            public async Task<CrearPeriodoVigenteResponse> Crear(CrearPeriodoVigenteRequest request)
            {
                //Obtener convocatoria
                var convocatoria = await _convocatoriaRepository.GetByIdAsync(request.ConvocatoriaId);
                if (convocatoria == null)
                    throw new ArgumentException("Convocatoria no encontrada");
    
                //Obtener periodo
                var periodo = await _periodoRepository.GetByIdAsync(request.PeriodoId);
                if (periodo == null)
                    throw new ArgumentException("Periodo no encontrado");

                await _convPeriodoRepository.DesactivarOtrosPeriodos();


                //Crear convocatoria periodo
                var convPeriodo = new ConvocatoriaPeriodo();
                convPeriodo = ConvocatoriaPeriodo.Crear(request.ConvocatoriaId, request.PeriodoId);


                await _convPeriodoRepository.AddAsync(convPeriodo);
                await _unitOfWork.SaveChangesAsync();

                return new CrearPeriodoVigenteResponse
                {
                    ConvocatoriaId = convPeriodo.ConvocatoriaId,
                    PeriodoId = convPeriodo.PeriodoId,
                    FechaInicio = periodo.FechaInicio,
                    FechaFin = periodo.FechaFin,
                    PeriodoNombre = periodo.Orden + " Convocatoria " + periodo.Cuatrimestre + " Cuatrimestre " +  periodo.Anio
                };


        }

    }
}
