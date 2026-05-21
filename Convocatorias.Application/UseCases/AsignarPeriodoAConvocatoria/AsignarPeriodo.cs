using Convocatorias.Application.Interfaces;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;


namespace Convocatorias.Application.UseCases.AsignarPeriodoAConvocatoria
{
    public class AsignarPeriodo
    {
        private readonly IConvPeriodoRepository _convPeriodoRepository;
        private readonly IConvocatoriaRepository _convocatoriaRepository;
        private readonly IPeriodoRepository _periodoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AsignarPeriodo(IConvPeriodoRepository convPeriodoRepository, IConvocatoriaRepository convocatoriaRepository, IPeriodoRepository periodoRepository, IUnitOfWork unitOfWork)
        {
            _convPeriodoRepository = convPeriodoRepository;
            _convocatoriaRepository = convocatoriaRepository;
            _periodoRepository = periodoRepository;
            _unitOfWork = unitOfWork;
        }

            public async Task<AsignarPeriodoResponse> Asignar(AsignarPeriodoRequest request)
            {
                //Obtener convocatoria
                var convocatoria = await _convocatoriaRepository.GetByIdAsync(request.ConvocatoriaId);
                if (convocatoria == null)
                    throw new ArgumentException("Convocatoria no encontrada");

                if(!convocatoria.estaAbierta())
                    throw new ArgumentException("La convocatoria esta cerrada, no se pueden asignar periodos");

                //si los periodos de la convocatoria no estan vacios, validar que el ultimo periodo cargado no este vigente
                if (convocatoria.Periodos.Any())
                {
                    var ultimoPeriodo= convocatoria.Periodos.FirstOrDefault(p => p.EsActual);
                    if (ultimoPeriodo != null)
                    {
                        var periodoActual = await _periodoRepository.GetByIdAsync(ultimoPeriodo.PeriodoId);
                        if (periodoActual.EstaVigente(DateTime.UtcNow))
                            throw new ArgumentException("El periodo actual de la convocatoria esta vigente, no se pueden asignar nuevos periodos");
                    }
                }


                //Obtener periodo
                var periodo = await _periodoRepository.GetByIdAsync(request.PeriodoId);
                if (periodo == null)
                    throw new ArgumentException("Periodo no encontrado");

                //Validar que el periodo este vigente
                if (periodo.EstaVigente(DateTime.UtcNow)==false)
                    throw new ArgumentException("El periodo seleccionado no esta vigente");

                //Desactivar otros periodos vigentes para la convocatoria solo puede haber uno activo por materia
                //await _convPeriodoRepository.DesactivarOtrosPeriodos(request.ConvocatoriaId);
                



            //Ver que la convocatoria no tenga el mismo periodo asignado
            if (convocatoria.Periodos.Any(p => p.PeriodoId == request.PeriodoId))
                    throw new ArgumentException("La convocatoria ya tiene asignado el periodo seleccionado");

                //Crear convocatoria periodo
                var convPeriodo = ConvocatoriaPeriodo.Crear(request.ConvocatoriaId, request.PeriodoId);
                convocatoria.Periodos.ToList().ForEach(p => p.MarcarComoNoActual());
                convocatoria.AgregarPeriodo(convPeriodo);
                await _convPeriodoRepository.AddAsync(convPeriodo);
                await _unitOfWork.SaveChangesAsync();

                return new AsignarPeriodoResponse
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
