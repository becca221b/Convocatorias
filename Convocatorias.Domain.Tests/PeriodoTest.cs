using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;

namespace Convocatorias.Domain.Tests;

public class PeriodoTest
{
    [Fact]
    public void CrearPeriodoVigente()
    {
        var periodo = new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
        Assert.NotNull(periodo);
        Assert.True(periodo.EstaVigente(DateTime.UtcNow));
    }

    [Fact]
    public void PeriodoVigenteDebeEstarDentroDelAnioActual()
    {
        var periodo = new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(10));
        Assert.True(periodo.EstaVigente(DateTime.UtcNow));
        Assert.True(periodo.FechaInicio <= DateTime.UtcNow && periodo.FechaFin >= DateTime.UtcNow);
        Assert.True(periodo.FechaFin.Year == DateTime.UtcNow.Year);
    }

    [Fact]
    public void PeriodoFueraDelAnioActualNoDeberiaEstarVigente()
    {
        var periodo = new Periodo(1, Cuatrimestre.Primer, 2024, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(10));
        Assert.False(periodo.EstaVigente(DateTime.UtcNow));
        
    }

    [Fact]
    public void CrearPeriodoConFechasInvalidasDebeLanzarExcepcion()
    {
        Assert.Throws<ArgumentException>(() => new Periodo(1, Cuatrimestre.Primer, 2026, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(-1)));
    }
}
