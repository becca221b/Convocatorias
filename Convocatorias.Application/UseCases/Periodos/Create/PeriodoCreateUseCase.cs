using Convocatorias.Application.Interfaces;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Domain.Entities;


namespace Convocatorias.Application.UseCases.Periodos.Create
{
    public sealed class PeriodoCreateUseCase
    {
        private readonly IPeriodoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PeriodoCreateUseCase(IPeriodoRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PeriodoResponse> HandleAsync(PeriodoCreateRequest request)
        {
            var periodo = new Periodo
            (
                orden: request.Orden,
                cuatrimestre: request.Cuatrimestre,
                anio: request.Anio,
                fechaInicio: request.FechaInicio,
                fechaFin: request.FechaFin

            );
            await _repository.AddAsync(periodo);
            await _unitOfWork.SaveChangesAsync();
            return new PeriodoResponse(
                periodo.Id,
                periodo.Orden,
                periodo.Cuatrimestre.ToString(),
                periodo.Anio,
                periodo.FechaInicio,
                periodo.FechaFin,
                EstaVigente: periodo.EstaVigente(DateTime.UtcNow)
            );
        }
    }
}
