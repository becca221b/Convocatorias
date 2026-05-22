using Convocatorias.Domain.Entities;

namespace Convocatorias.Domain.Tests;


public class ConvocatoriaPeriodoTest
{
    [Fact]
    public void cuando_Se_crea_un_ConvocatoriaPeriodo_EsActual()
    {
        // Arrange
        var convocatoriaId = Guid.NewGuid();
        var periodoId = Guid.NewGuid();
        // Act
        var convocatoriaPeriodo = ConvocatoriaPeriodo.Crear(convocatoriaId, periodoId);
        // Assert
        Assert.True(convocatoriaPeriodo.EsActual);
    }

    [Fact]
    public void cuando_Se_marcar_como_no_actual_un_ConvocatoriaPeriodo_EsActual_es_falso()
    {
        // Arrange
        var convocatoriaId = Guid.NewGuid();
        var periodoId = Guid.NewGuid();
        var convocatoriaPeriodo = ConvocatoriaPeriodo.Crear(convocatoriaId, periodoId);
        // Act
        convocatoriaPeriodo.MarcarComoNoActual();
        // Assert
        Assert.False(convocatoriaPeriodo.EsActual);
    }
    

}
