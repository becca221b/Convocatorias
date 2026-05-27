using Convocatorias.Application.Interfaces;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;


namespace Convocatorias.Application.UseCases.AsignarPeriodoAConvocatoria
{
    public class AsignarPeriodo
    {
        
        private readonly IConvocatoriaRepository _convocatoriaRepository;
        private readonly IPeriodoRepository _periodoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AsignarPeriodo(IConvocatoriaRepository convocatoriaRepository, IPeriodoRepository periodoRepository, IUnitOfWork unitOfWork)
        {
            
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


                //Ver que la convocatoria no tenga el mismo periodo asignado
                if (convocatoria.Periodos.Any(p => p.PeriodoId == request.PeriodoId))
                        throw new ArgumentException("La convocatoria ya tiene asignado el periodo seleccionado");

                //Crear convocatoria periodo             
                convocatoria.AgregarPeriodo(request.PeriodoId);

                // Persistir el agregado modificado en el repositorio antes de commitear la UoW
                await _convocatoriaRepository.UpdateAsync(convocatoria);

                await _unitOfWork.SaveChangesAsync();

                return new AsignarPeriodoResponse
                {
                    ConvocatoriaId = convocatoria.Id,
                    PeriodoId = convocatoria.ObtenerPeriodoActual(),
                    FechaInicio = periodo.FechaInicio,
                    FechaFin = periodo.FechaFin,
                    PeriodoNombre = periodo.Orden + " Convocatoria " + periodo.Cuatrimestre + " Cuatrimestre " +  periodo.Anio
                };

                
            }

    }
}
