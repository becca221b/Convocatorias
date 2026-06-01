using Convocatorias.Application.Interfaces;
using Convocatorias.Application.Interfaces.Repositories;
using Convocatorias.Application.UseCases.Convocatorias.GetAll;
using Convocatorias.Domain.Entities;
using Convocatorias.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Application.UseCases.Convocatorias.Create
{
    public sealed class ConvCreateUseCase
    {
        private readonly IConvocatoriaRepository _repository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IPeriodoRepository _periodoRepository;
        public ConvCreateUseCase(IConvocatoriaRepository repository, IUnitOfWork unitOfWork, IPeriodoRepository periodoRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _periodoRepository = periodoRepository;
        }
        public async Task<ConvocatoriaResponse> ExecuteAsync(ConvCreateRequest request, CancellationToken ct = default)
        {
            // Aquí iría la lógica para crear una nueva convocatoria, por ejemplo:
            var periodoVigente = await _periodoRepository.GetVigenteAsync(DateTime.UtcNow);

            // 2. Crear una nueva entidad de convocatoria
            var nuevaConvocatoria = new Convocatoria
            (
                
                sedeId: request.SedeId,
                facultadId: request.FacultadId,
                carreraId: request.CarreraId,
                asignatura: request.Asignatura,
                modalidad: request.Modalidad,
                periodoInicial: periodoVigente
            );
            // 3. Guardar la entidad en la base de datos a través del repositorio
            await _repository.AddAsync(nuevaConvocatoria, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            // 4. Retornar el Id de la nueva convocatoria creada
            return new ConvocatoriaResponse
            (
                Id: nuevaConvocatoria.Id,
                Asignatura: nuevaConvocatoria.Asignatura,
                SedeId: nuevaConvocatoria.SedeId,
                FacultadId: nuevaConvocatoria.FacultadId,
                CarreraId: nuevaConvocatoria.CarreraId,
                Modalidad: nuevaConvocatoria.Modalidad.ToString(),
                Status: nuevaConvocatoria.Status.ToString()
            );


        }
    }
}
