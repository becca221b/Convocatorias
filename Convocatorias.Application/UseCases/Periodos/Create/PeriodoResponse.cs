

namespace Convocatorias.Application.UseCases.Periodos.Create
{
    public sealed record PeriodoResponse(
        Guid Id,
        int Orden,
        string Cuatrimestre,
        int Anio,
        DateTime FechaInicio,
        DateTime FechaFin,
        bool EstaVigente
    );
}
